using EventScheduler.Common.Exceptions;
using EventScheduler.Data;
using EventScheduler.Services.Exceptions;
using EventScheduler.Services.Model.Event;
using EventScheduler.Services.Model.Participant;
using EventScheduler.Services.Model.Speaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EventScheduler.Services.Services
{
    public interface IEventDataService
    {
        Guid AddEvent(string userId, NewEventModel data);
        Guid AddParticipant(string userId, NewParticipantModel data);
        Guid AddSpeaker(string userId, NewSpeakerModel data);
        void DeleteEvent(string userId, Guid eventId);
        void DeleteParticipant(string userId, Guid participantId);
        void DeleteSpeaker(string userId, Guid speakerId);
        IEnumerable<object> GetEvents(string userId);
        IEnumerable<ParticipantViewModel> GetParticipantsByEvent(string userId, Guid eventId);
        IEnumerable<SpeakerViewModel> GetSpeakersByEvent(string userId, Guid eventId);
        IEnumerable<object> GetExcernalEvents(string userId);
    }

    public class EventDataService : IEventDataService
    {
        ApplicationDbContext _db;
        public EventDataService(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<object> GetEvents(string userId)
        {
            return _db.Events.Where(z => z.UserId == userId).Select(e => new EventViewModel
            {
                Id = e.Id,
                Description = e.Description,
                UserId = e.UserId,
                End = e.End,
                IsOnline = e.IsOnline,
                Location = e.Location,
                MaxCapacity = e.MaxCapacity,
                Name = e.Name,
                Start = e.Start,
                ParticipantCount = e.Participants.Count,
                SpeakerCount = e.Speakers.Count

            }).ToList(); ;
        }
        public void DeleteEvent(string userId, Guid eventId)
        {
            var eventToDelete = _db.Events.SingleOrDefault(z => z.UserId == userId && z.Id == eventId);
            if (eventToDelete == null) throw new Exception("Event not found");
            _db.Events.Remove(eventToDelete);
            _db.SaveChanges();
        }
        public Guid AddEvent(string userId, NewEventModel data)
        {
            var newEvent = new EventScheduler.Data.Model.Event
            {
                Id = Guid.NewGuid(),
                Name = data.Name,
                Description = data.Description,
                End = data.End,
                IsOnline = data.IsOnline,
                Location = data.Location,
                MaxCapacity = data.MaxCapacity,
                Start = data.Start,
                UserId = userId
            };
            _db.Events.Add(newEvent);
            _db.SaveChanges();
            return newEvent.Id;
        }

        public IEnumerable<ParticipantViewModel> GetParticipantsByEvent(string userId, Guid eventId)
        {
            return _db.Participants.Where(z => z.Event.UserId == userId && z.EventId == eventId).Select(p => new ParticipantViewModel
            {
                Email = p.Email,
                EventId = p.EventId,
                FirstName = p.FirstName,
                Id = p.Id,
                LastName = p.LastName,
                Occupation = p.Occupation
            }).ToList();
        }
        public Guid AddParticipant(string userId, NewParticipantModel data)
        {
            var eventToAddParticipant = _db.Events.Where(z => z.UserId == userId && z.Id == data.EventId).Select(z => new
            {
                NrOfParticipants = z.Participants.Count,
                MaxParticipantsAllowed = z.MaxCapacity
            }).SingleOrDefault();
            if (eventToAddParticipant == null) throw new Exception("Event not found");
            if (eventToAddParticipant.MaxParticipantsAllowed.HasValue && eventToAddParticipant.MaxParticipantsAllowed.Value >= eventToAddParticipant.NrOfParticipants) throw new Exception("Maximum capacity exceeded");
            var newParticipant = _db.Participants.Add(new Data.Model.Participant
            {
                Id = Guid.NewGuid(),
                Email = data.Email,
                EventId = data.EventId,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Occupation = data.Occupation
            }).Entity;
            _db.SaveChanges();
            return newParticipant.Id;
        }
        public void DeleteParticipant(string userId, Guid participantId)
        {
            var participant = _db.Participants.SingleOrDefault(z => z.Event.UserId == userId && z.Id == participantId);
            if (participant == null) throw new Exception("Participant not found");
            _db.Participants.Remove(participant);
            _db.SaveChanges();
        }
        public IEnumerable<SpeakerViewModel> GetSpeakersByEvent(string userId, Guid eventId)
        {
            return _db.Speakers.Where(z => z.Event.UserId == userId && z.EventId == eventId).Select(p => new SpeakerViewModel
            {
                EventId = p.EventId,
                FirstName = p.FirstName,
                Id = p.Id,
                LastName = p.LastName,
                Occupation = p.Occupation,
                Linkedin = p.Linkedin,
                Twitter = p.Twitter,
                Website = p.Website,

            }).ToList();
        }
        public Guid AddSpeaker(string userId, NewSpeakerModel data)
        {

            if (_db.Events.Any(z => z.UserId == userId && z.Id == data.EventId)) throw new Exception("Event not found");

            var newSpeaker = _db.Speakers.Add(new Data.Model.Speaker
            {
                Id = Guid.NewGuid(),
                EventId = data.EventId,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Occupation = data.Occupation,
                Twitter = data.Twitter,
                Linkedin = data.Linkedin,
                Website = data.Website
            }).Entity;
            _db.SaveChanges();
            return newSpeaker.Id;
        }
        public void DeleteSpeaker(string userId, Guid speakerId)
        {
            var speaker = _db.Speakers.SingleOrDefault(z => z.Event.UserId == userId && z.Id == speakerId);
            if (speaker == null) throw new Exception("Speaker not found");
            _db.Speakers.Remove(speaker);
            _db.SaveChanges();
        }

        public IEnumerable<object> GetExcernalEvents(string userId)
        {
            //For demo purposes only.
            //This method gets a list of events from google calendar. Here we would call the google api and if something goes wrong we return a custom exception
            //For demo purposes we are only going to throw the error to demonstrate the error
            throw new DefaultEventSchedulerExceptionTwo
            {
                 ErrorCode=10,
                 HttpStatusCode= HttpStatusCode.FailedDependency,
                 Message="Failed to call google api"
            };
        }
    }
}
