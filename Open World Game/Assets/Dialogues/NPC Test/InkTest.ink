Welcome in my own dialogue tutorial # Narrator

Let's start off with a simple choice # Narrator

// This is a choice that can only be chosen once
    * Choice 0 # Steve0
    * Choice 1 # Steve1
    
- Good Job # Narrator

-> knot

==knot==
Now just try not to stick with the same anwer # Narrator

// This is a sticky choice - the player can choose it more than once
    + Sticky choice 0 # Steve
        -> knot
    + Sticky choice 1 # Steve
        -> knot
    * Normale choice # Steve

- Nice. Keep on with the good work # Narrator

This time be careful, I won't give you a feedback for your answer # Narrator # Steve0 #Steve1
// [A choice where the content isn't printed after choosing]
    * [Invisible choice 0]
    * [Invisible choice 1]
    
- Well, I hope you chose well # Narrator

Last but not least I'll just correct you no matter what # Narrator
// Input and outpu are slightly different
    * I choose [1] this # Steve
    * I choose [2] that # Steve
    
-->END