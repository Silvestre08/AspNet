# Authentication and Authorizarion in ASP.NET

APIs manage data and or business logic for applications; therefore they are often the heart of an organization!
As such they need to be protected.
Our application explained:

Our application is for a fictional company that manages conferences. Speakers send talk proposals.
Proposals can be added and approved.

# Protecting APIs with keys

The most basic way of protecting APIs is by using a key.
A key is like a password that must be supplied by the consumer of an API to gain access.
The consumer is typically another application. An http header is often used to do that. The browser in our api has nothing to do with a key (stored in the API.). It is just used for machine to machine.

Problems

1. Keys can easily be stolen (usually laying around excel sheets, etc.)
1. Tend to have no expiraltion (hard to revoke when the attacker gets it).
1. All parties that have the key would have to rotate at the same time.
1. No middleware that supports API keys.

There are often better, more modern ad secure options. It is just mentioned here for infomation and completeness purposes.
