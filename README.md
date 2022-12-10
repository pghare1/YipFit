
# YipFit

A Fitness App that curates a workout session to any User based on their immediate fitness goals! 

### Who’s it for?

While the target audience is between the 15-55 years age demographic it should also entice fitness enthusiasts of any age.

### Main Features
Gamelib as a package will need to be imported before we approach any of the following requirements

- #### The Aesthetic and Scene design 

    Intro: Storyboard a 3~5s opener revealing the app logo. This will be the hyped animation intro that should successfully draw anyone in.
    
    Landing page (main): translucent Ui for Start, Settings, Accolades, Exit. There will be a minimal palette Each button will have its layout. A Live Flipboard tracker of overall local/global Calorie burned (TBD). 
    
    Game screen (main): Active from start to end of a session we will see on screen the following as display Ui -  a current workout timer, the name of the current workout, overall timer for the session duration and number of actions per workout (actions performed will be shown after that particular workout duration).
    
    Results screen (main): Fitness report card screen that conveys the players’ workout summary stats, calories burned, total time and fitness points  
    
    Feedback screen (TBD): a quick survey questionnaire to get the user to rate his session. A different question will appear each time so the user will not be bor could also be used to revise the algorithm to make the user  

- #### Character design 

    The 3D models will be both male and female humanoid body types. There will not be any facial rigs for the sake of simplicity. However, the attire of the models will follow the aesthetics of fitness trainers, gym instructors and showcase traits from familiar coach personalities (Peloton, Insanity, P-90X) through specific animation actions (TBD).

- #### The Action Library 

    A complete list of 10-15 workout actions including variations with accurate stats. The library of animations can then be derived from this list. 
    
    Any peripheral (non-workout) actions can also be created such as welcome gestures for the landing page, workout start and cooldowns, breaktime poses and workout completion poses.
    
    Besides the main core animations, we can also aim to add some quirkiness to the character models by having them mimic the movements of popular trainers or famous coaches. Users may enjoy the familiarity of recognizable personalities. (TBD) 

- #### Algorithm Managers

    Create a Configurations manager to handle the users’ data inputs such as time, intensity and body focus. Its macro function is to pair with other entities such as a Character manager to handle which animations are selected, a UI manager from the start to the end of a workout session, a Sound Manager, A TTS manager that manages the feedback dialogues during and after the session completion.

- #### Music, Dialogues & SFX 

    Music along with Feedback dialogues will be another important pillar that will enhance if not compliment the fitness experience. 
    
    There will be a unique tracklist curated for various segments throughout the app’s navigation. The Intro, the main menu navigation, a tracklist of uptempo beats specifically for Low, Moderate and High tempo workouts. 
    
    There will be a common sfx library for sounds for various button clicks, milestone updates, page navigations and screen transitions, workout round start and end segments, breaktime solos, countdown timer riffs. 
    
    Completing an entire session, will bring up a workout summary page and will follow up with a short feedback survey to the user before exiting. Transition between the screen pages will have a creative sfx flow in with the music.  
    
    All the Feedback dialogues will be carefully curated to guide the user throughout their workout session, constantly encouraging and motivating them before, during and after completing every workout. To keep things interesting we could use funny and quirky TTS dialogues from the aforementioned fitness personalities. Let’s not make this just another fitness app. 

### Extra Features

- Personalized workouts: Allow users to create their own preferred workouts
- Video links: Allow users to also view the workouts available on the interweb
- Mat gestures (TBD): UX component that allows users to press trigger points on the mat




