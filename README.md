
# YipFit

A Fitness App that curates a workout session to any User based on their immediate fitness goals! 
While the target audience is between the 15-55 years age demographic it should also entice fitness enthusiasts of any age.

# YipFit Folder Structure
The following are the lists of folders where we maintain
our project. You can also see the
purpose of each particular folder.
The project folder is located inside Assets/PlayFit/

## All Parent Folders

- ### Animations
        This folder includes all the animations relating to the UI of the app.

- ### Character
        The folder includes the 3D model file of the character which is being used inside the project.
        There are animations of the characters within this folder which is used to create the animators of the character
        Also, it includes the material of the character which consists the skin, clothes and other accessories.
    
- ### Fonts
        All the fonts files which are currently being used in the project

- ### Material
        All the other materials which are used for the environment assets
    
- ### Models
        All the models relating to the environment assets

- ### Resources
        It includes the .bundle files which are used as a Downloadable Content for audio

- ### Scenes
        It includes the main scenes where the project will be executed and rendered
    
- ### Scripts
        All the namespaces and classes are stored here

- ### SFX
        All kinds of sound effects and dialgoues for the project are stored here.

- ### Sprites
        All the images relating to UI and 2D content are stored in here.

- ### Textures
        All the textures relating to the environment assets

- ### UI
        Other miscellanous types of UI animations are stored here


## All the Namespaces and their Classes

- ### CharacterManager
        - CharacterAnimationEventHandler.cs contains the Key Events functions for the character animation. Events will be triggered when a certain frame/time of the animation is being played.

        - CharacterAnimationHandler.cs stores the meta list information of the animations, it include other functionality also to set the speed of the animation. The requested animation is sent from this meta list.

        - CharacterManager.cs includes the funationality like to Apply Idle animation, Apply the animation override, Display the character and Hide the character

- ### CurriculumGeneration

        - The curriculum generation generates the workout on run-time basis depending upon the user selection on time and intensity.
        The following scripts are being used for generation the workout curriculum:
        -> ActionMappedTargetAreaAndIntensity.cs This table will define the % wise distribution among the 3 workout sections, depending upon the intensity selected.
        -> CurriculumGenerator.cs this script generates the workout based on the user input
        -> RunTimePredefinedWorkout.cs this script holds the information of workout created by CurriculumGenerator in metadata format.
        -> SectionAndDuration.cs
        -> WorkoutTimeAndIntensity.cs

- ### Database

        - BackendDataHandler.cs handles the user data information and also handles the information of the workout taken by the user. All the fetching and storing of the data is handled by this script. The database used is Firebase Realtime Database

- ### DLCModule
        - This component is used to manage the audio as a downloadable content. When the app launches it fetches all the audio content from storage server using FTP request and then store it to local cache storage of device and on re-launch of the app the content will be fetched from the cache storage.

- ### GameInitaliser
        - Intializes the game with its id and game type

- ### UIManager
        - AudioClipHolder.cs holds the audio data in meta format when the DLCModule has downloaded the content successfully.
        - CheckPointManager.cs manages the trigger point for the workout, i.e. when the break, motivation audio, change of background music and engaging audio will be invoked.
        - SoundManager.cs select the sounds from the AudioClipHolder depending on the which section of the workout is being played.
        - UIManager.cs manages all the inteactable UI when users gives the input or clicks/touches.

# Store Link

[Play Store](https://play.google.com/store/apps/details?id=com.yipli.yipfit)

[App Store](https://apps.apple.com/in/app/yipfit/id1621274138)







