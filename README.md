# Winfrog Waypoint Helper

Enter in positions as Eastings,Northings or optionally as Name,Eastings,Northings and this program will rejig it into a string that Winfrog can read from to save you CONCATENATE'ing time in Excel, or god forbid actually doing them one by one with the default 'Add New Waypoint' dialog.

Copy the new string to the bottom of the Waypoint.wpt file then get Winfrog to reload it (either by restart, or File>Select Working Files and choose a blank one, then choose your Waypoint.wpt again).

There is no geodesy involved in this program, it simply adds in zeroed out Lat/Long positions which Winfrog handily ignores, reads Eastings/Northings then computes the Lat/Long to save back into the waypoint file.

ToDo:

Only allow valid numerical values in the Depth/Radius boxes for the fatter fingered surveyors