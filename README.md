# Fuzzphyte Unity Tools

## Control

FP_Control is designed and built to help quickly modify the existing [Unity Character Controller](https://docs.unity3d.com/ScriptReference/CharacterController.html) class. It's based on a humble design pattern utilizing a scriptable object to pass data back to the controller. This setup is easily modified but locked within basic movements. This version is initially designed for 3D control but can be used for 2D by adjusting and zeroing out the inputs.

## Setup & Design

FP_Control is designed to be driven by a data file and requires a unity character controller component. It has dependencies to the new Unity Input System - but it's not needed. This dependency is only for the sample files. Currently the core of the runtime scripts are tied to the 'PlayerController.cs' class which is a standard C# class not derived from mono. In order to showcase how to utilize and use this pattern: Please install the samples when setting up this package within the Unity package manager. The concept is to eventually allow users to quickly generate multiple character controllers as needed and to be able to generate them on the fly during runtime. In the unreleased version 0.1.0 we are getting the initial foundational pieces put together.

**NOTE** By using the Unity Character Controller we are allowing Unity to process a lot of the collisions for us but be aware that the Character Controller by itself doesn't implement 'physics' this code utilizes the underlying physics information and does simple gravity checks and simple forces for jumping. **MAKE SURE** your character controller is on a different layer than your ground. Currently the sample resources have a simple implementation of ground checks and it's assuming it cannot collide with itself - by assuming the player/character controller and the ground are on different layers. If you notice that gravity isn't acting 'right' check your layers. I will flush out a better solution - but once you go down the Unity Character Controller class... you're already putting your hands up :smile:!

### Software Architecture

There are currently three parts to the FP_Control tool.

* Character Controller Data ScriptableObjects under the 'SO_ControlParameters.cs' file
  * This data file represents all of the information needed to adjust, move, jump, and account for the default Unity Character Controller setup
* PlayerController.cs which manages all of the data and behavior functions, this class is designed to work with a mono class.
* IFPControl.cs interface: which helps implement the [Humble pattern](https://www.youtube.com/watch?v=3O_rpTWdGps) based on some of the work by Jason Weimann. This interface should be used by a mono class that you ultimately create on your own - but one is provided in the samples
  * In the samples: see the FP_Control_Demo scene and how the FP_Controller.cs script is setup to work with the IFPControl.cs interface, the different input systems, and the PlayerController.cs script

### Ways to Extend

* You should be able to easily implement this within a prefab approach in which you could write additional code to instantiate a generic prefab character controller and pass in the data (scriptable object) file on setup.
* Rewrite your own humble pattern from this design and just get rid of this package entirely :smile:
  * Most of the core controller code is based on simple use cases - you can generate and derive from the base and override all the functions you want to.

## Dependencies

Please see the [package.json](./package.json) file for more information.

## License Notes

* This software running a dual license
* Most of the work this repository holds is driven by the development process from the team over at Unity3D :heart: to their never ending work on providing fantastic documentation and tutorials that have allowed this to be born into the world.
* I personally feel that software and it's practices should be out in the public domain as often as possible, I also strongly feel that the capitalization of people's free contribution shouldn't be taken advantage of.
  * If you want to use this software to generate a profit for you/business I feel that you should equally 'pay up' and in that theory I support strong copyleft licenses.
  * If you feel that you cannot adhere to the GPLv3 as a business/profit please reach out to me directly as I am willing to listen to your needs and there are other options in how licenses can be drafted for specific use cases, be warned: you probably won't like them :rocket:

### Educational and Research Use MIT Creative Commons

* If you are using this at a Non-Profit and/or are you yourself an educator and want to use this for your classes and for all student use please adhere to the MIT Creative Commons License
* If you are using this back at a research institution for personal research and/or funded research please adhere to the MIT Creative Commons License
  * If the funding line is affiliated with an [SBIR](https://www.sbir.gov) be aware that when/if you transfer this work to a small business that work will have to be moved under the secondary license as mentioned below.

### Commercial and Business Use GPLv3 License

* For commercial/business use please adhere by the GPLv3 License
* Even if you are giving the product away and there is no financial exchange you still must adhere to the GPLv3 License

## Contact

* [John Shull](mailto:the.john.shull@gmail.com)
