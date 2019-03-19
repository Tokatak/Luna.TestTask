# Luna.TestTask

Prototype for porting Unity Yaml scene files to WebGL.
Provides wrappers and a set of basic interpreters to transfer unity scene objects to data for ThreeJs.
Flow:
Yaml scene split in components => serialized => wrapped in data container => transfered to Adapter that uses Interpreters to buld a ThreeJs scene file => Data loaded to ThreJs using it's API.

<img src="https://user-images.githubusercontent.com/13577949/54582272-cf5a8f80-4a18-11e9-82a6-9611d780be24.png" width="300" height="300"><img src="https://user-images.githubusercontent.com/13577949/54582274-d08bbc80-4a18-11e9-9294-b826cafc5685.png" width="300" height="300">


### Prerequisites

Unity  2018.3.6f1 + shoud do.

## Running 

![index](https://user-images.githubusercontent.com/13577949/54581078-b9969b80-4a13-11e9-9fe7-d71ab44cf1e9.png)

**Warning in case missing, reimport Dependency/Wizard folder.**

![index2](https://user-images.githubusercontent.com/13577949/54581221-653feb80-4a14-11e9-9952-fae1ff568d43.png)

After port build up page shoud appear at [ProjectFolder]\Builds

## Built With

* [Unity](https://unity.com/) - Unity.
* [ThreeJS](https://threejs.org/) - Web render template.
* [Newtonsoft.Json](https://www.newtonsoft.com/json) - Serializing Scene wrappers.
* [YamlDotNet](https://github.com/aaubry/YamlDotNet) - Great Yaml serializer/deserializer.
