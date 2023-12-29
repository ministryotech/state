A selection of libraries, each providing different base state storage implementations that become easily interchangeable.

This replaces the traditional string dictionary based methods of accessing state storage which, when used out of the box, make
testing impossible with a solid mockable interface and alternative implementations to swap out web session with in memory state, for example.

## WebSession ##
### Using Session State in your Application ###
Use a WebSessionStateBase class. Recommended usage is to inherit from this class in your application and use that class to access session state. You can then swap out your session wrapper with any other implementation of IStateStorage to enable testing of components that previously would have been extremely hard to test because of a dependency on session state.


### Faking WebSession ###
The IStateStorage interface provides an in-memory alternative to Session that can be used within your tests to remove a dependency on session state by using a custom implementation using InMemoryStateBase.

## Cookies ##
The CookieStateBase class provides an IStateStorage base implementation that stores and retrieves values from cookies directly.

## Application State ##
Application state can be accessed using exactly the same methodology by inheriting from the ApplicationStateBase class.

## Composite State ##
IStateStorage is non judgemental - It simply provides a way by which you can access and save data. If you want a complex state storage mechanism for your application, you could create your own implementation of IStateStorage with strongly typed properties for all of your key data items and then, internally, use various other IStateStorage implementations to store different items in different places, making use of Web Session, Cookies and Application state within a single State Repository.

If you extend the IStateStorage interface with your own interface to cover the new properties that you add to it you can then easily create a fake implementation using InMemorystateBase class for your testing.

This is also an extensible model. You can start simply with your own Session implementation extending WebSessionBase but then change your mind and make it composite - none of the consuming code would need to be changed at all.

## Versions to Use
Use v1.x for AspNet Core 1
Use v2.x for AspNet Core 2
Use v6.x for AspNet Core in net 6 and above

## The Ministry of Technology Open Source Products
Welcome to The Ministry of Technology open source products. All open source Ministry of Technology products are distributed under the MIT License for maximum re-usability.
Our other open source repositories can be found here...

* [https://github.com/ministryotech](https://github.com/ministryotech)
* [https://github.com/tiefling](https://github.com/tiefling)

### Where can I get it?
You can download the package for this project from any of the following package managers...

- **NUGET** - [https://www.nuget.org/packages/Ministry.State](https://www.nuget.org/packages/Ministry.State)

### Contribution guidelines
If you would like to contribute to the project, please contact me.

### Who do I talk to?
* Keith Jackson - temporal-net@live.co.uk