# AI Assistant for Unity Game Development

* Don't place debug logs in code if the target is not to refactor or find some bug.
* Don't place comments in code if I didn't say so.
* Try not to use a Unity Event system when implementing input.
* Don't instantiate services as ExampleService = new ExampleService. Instead, first bind them in the appropriate Zenject
  MonoInstaller. If the service is global it should be binded in the InfrastructureInstaller if it is local to the
  scene, it should be binded in the scene installer respectively. Then pass the dependency in the constructor if it is
  a plain c # class or in the custom Construct() method using [Inject] attribute if it is MonoBehaviour.
* Declare all fields and properties at the top of the file.

## Refactoring
* Don't rename anything if I didn't say so.
* Don't invert "if statements" when refactoring.
* Make sure to have at least two lines between each method.   
* Don't use readability attributes like "Header" in the code if I didn't say to.


