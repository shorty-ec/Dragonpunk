// Title and intro
=== title ===
Welcome to *The Journey Begins*
-> start

=== start ===
You find yourself at the edge of a dense forest, the path stretching out before you.
* [Enter the forest] -> forest_path
* [Turn back] -> turn_back

=== forest_path ===
You step into the forest. It's dark, and the sounds of the woods surround you.
* [Move deeper] -> deeper_forest
* [Climb a tree to look around] -> climb_tree

=== turn_back ===
Are you sure you want to turn back?
* [Yes, I'm not ready for this adventure.] -> end
* [No, I'll go back to the forest path.] -> forest_path

=== deeper_forest ===
As you walk, you spot a fork in the path.
* [Take the left path] -> left_path
* [Take the right path] -> right_path

=== climb_tree ===
You find a sturdy tree and climb up. From here, you can see smoke rising in the distance.
* [Head towards the smoke] -> smoke_path
* [Ignore it and continue] -> deeper_forest

=== left_path ===
You hear strange whispers, and suddenly a wild creature appears!
-> fight_or_flee

=== right_path ===
You stumble upon an abandoned campsite.
-> campsite

=== smoke_path ===
You approach the source of the smoke and find a small campfire with a traveler.
"Greetings," says the traveler. "Care to join me?"
* [Sit down by the fire] -> campfire_talk
* [Politely decline and move on] -> deeper_forest

=== fight_or_flee ===
Do you want to fight or flee?
* [Fight the creature] -> fight
* [Flee from the creature] -> flee

=== fight ===
You engage the creature! Roll for a strength check.
+5 to strength.
-> deeper_forest

=== flee ===
You manage to escape back to safety.
-> deeper_forest

=== campfire_talk ===
You share stories with the traveler, and they give you a map of the forest.
+1 item: map
-> deeper_forest

=== campsite ===
The campsite is empty but you find some useful supplies.
+3 health potions
-> deeper_forest

=== end ===
Thank you for playing! The journey may have ended here, but the forest awaits whenever you're ready.

-> END