# SimpleInjector.AutoRegistration
An Extensible Auto-Registration Extension for Simple Injector

## Summary
Name: KeithAKnight.SimpleInjector.AutoRegistration

Description: A flexible auto-registration module that extends Simple Injector 
Inversion of Control IoC container (https://simpleinjector.org).

By default, this will register the basic types with simple registrations, including
more complex open-generics.  It also adds support for your IEnumerable<>, Lazy<>, and Func<> dependencies.

Optionally, you can configure this to automatically register your decorators and you can extend
or superceded it with your own custom auto-registration providers.

Author: Keith A. Knight 

http://keithaknight.com

https://www.linkedin.com/in/keithaknight

https://github.com/keithaknight


## Setup
 
Add the KeithAKnight.SimpleInjector.AutoRegistration project or .dll to your solution.


## Usage

Add the following to your application's composition root:

To header:
using KeithAKnight.SimpleInjector.AutoRegistration;

To container egistration (where container is your Simple Injector Container instance):
container.AutoRegister();


## License

KeithAKnight.SimpleInjector.AutoRegistration is available under the MIT license.  See [License.txt] (https://github.com/keithaknight/SimpleInjector.AutoRegistration/blob/master/License.txt) for more details.
This software is not affiliated with nor endorsed by Simple Injector (https://simpleinjector.org).

Additional details may be found at: http://opensource.org/licenses/MIT.

