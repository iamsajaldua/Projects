sing System;
namespace Battleship
{
public class Program
{
public static void Main(string[] args)
{
//I think I'm gonna have an aneurysm because this thing actually works!
//Initializes user input vars and the RNG
Random rnd = new Random();
int res1;
int res2;
string res3;
Start:
//Initializes vars not controlled by the player
int shipsVal = 0;
int misses = 0;
int hitsVal = 0;
int botHitsVal = 0;
int botCarrHits = 0;
int botBattHits = 0;
int botCruiserHits = 0;
int botSubHits = 0;
int botDesHits = 0;
int strikes = 0;
int opX = 0;
int opY = 0;
string lastMark = null;
int checksVal;
Console.WriteLine("\nImportant notice:\nThis is a beta build.\nThe game should play smoothly,\nbut it cannot be guaranteed in this build.\n\nShip origins are the top left.\nEnjoy!\n\n");
//Initializes strings
string[] ships = new string[5];
string[] carr = new string[5];
string[] batt = new string[4];
string[] cruiser = new string[3];
string[] sub = new string[3];
string[] des = new string[2];
string[] botCarr = new string[5];
string[] botBatt = new string[4];
string[] botCruiser = new string[3];
string[] botSub = new string[3];
string[] botDes = new string[2];
string[] hits = new string[17];
string[] botHits = new string[17];
ships[0] = "carrier";
ships[1] = "battleship";
ships[2] = "cruiser";
ships[3] = "submarine";
ships[4] = "destroyer";
int width = 11;
int height = 10;
//Initializes 3 core grids
string[,] grid = new string[999, 999]; //numbers larger than display to prevent overflow
string[,] opGrid = new string[999, 999];
string [,] trueGrid = new string[999, 999];
GridDisplay: //tells game to draw grids; Probably should be a function but I don't understand those well
//Draws hostile grid
Console.WriteLine("    Hostile Grid\n    1 2 3 4 5 6 7 8 9 10");
for(int y = 0; y < height; y++){
for(int x = 0; x < width; x++){
opGrid[0, 0] = "1  ";
opGrid[0, 1] = "2  ";
opGrid[0, 2] = "3  ";
opGrid[0, 3] = "4  ";
opGrid[0, 4] = "5  ";
opGrid[0, 5] = "6  ";
opGrid[0, 6] = "7  ";
opGrid[0, 7] = "8  ";
opGrid[0, 8] = "9  ";
opGrid[0, 9] = "10 ";
if(opGrid[x, y] == null){
opGrid[x, y] = ".";
}
Console.Write(opGrid[x,y] + " ");
}
Console.WriteLine();
}
//Draws player grid
Console.WriteLine("\n\n    Player Grid\n    1 2 3 4 5 6 7 8 9 10");
for(int y = 0; y < height; y++){
for(int x = 0; x < width; x++){
grid[0, 0] = "1  ";
grid[0, 1] = "2  ";
grid[0, 2] = "3  ";
grid[0, 3] = "4  ";
grid[0, 4] = "5  ";
grid[0, 5] = "6  ";
grid[0, 6] = "7  ";
grid[0, 7] = "8  ";
grid[0, 8] = "9  ";
grid[0, 9] = "10 ";
if(grid[x,y] == null){
grid[x,y] = ".";
}
Console.Write(grid[x, y] + " ");
}
Console.WriteLine();
}
//Checks if player has finished setting ships or not
if(shipsVal > 4){
goto Attack;
}
Select:
//Initializes player setup (I don't think this label is actually used)
Console.WriteLine($"\nPlease select an X coordinate for your {ships[shipsVal]}.");
res1 = Convert.ToInt32(Console.ReadLine());
if(res1 == 0){
res1 = 10;
}
Console.WriteLine("\nNow select a Y.");
res2 = Convert.ToInt32(Console.ReadLine()) - 1;
if(res2 == -1){
res2 = 9;
}
Orient:
//A frankenswitch statement that checks orientation (Same doubts as above)
Console.WriteLine("Vertical or horizontal?\n(v/h)");
res3 = Console.ReadLine();
switch(shipsVal){
case 0:
switch(res3){
case "v":
if(res2 + 4 > 9){
Console.WriteLine("\nInvalid range.\nPlease reposition.");
grid[res1, res2] = ".";
goto Select;
}
 
grid[res1,res2] = "#";
grid[res1, res2 + 1] = "#";
grid[res1, res2 + 2] = "#";
grid[res1, res2 + 3] = "#";
grid[res1, res2 + 4] = "#";
carr[0] = grid[res1,res2];
carr[1] = grid[res1,res2 + 1];
carr[2] = grid[res1,res2 + 2];
carr[3] = grid[res1,res2 + 3];
carr[4] = grid[res1,res2 + 4];
break;
case "h":
if(res1 + 4 > 10){
Console.WriteLine("\nInvalid range.\nPlease reposition.");
grid[res1, res2] = ".";
goto Select;
}
grid[res1, res2] = "#";
grid[res1 + 1, res2] = "#";
grid[res1 + 2, res2] = "#";
grid[res1 + 3, res2] = "#";
grid[res1 + 4, res2] = "#";
carr[0] = grid[res1, res2];
carr[1] = grid[res1 + 1, res2];
carr[2] = grid[res1 + 2, res2];
carr[3] = grid[res1 + 3, res2];
carr[4] = grid[res1 + 4, res2];
break;
default:
Console.WriteLine("\nInvalid orient.\nTry again.");
goto Orient;
}
break;
case 1:
BattSelect:
switch(res3){
case "v":
if(grid[res1, res2] != "." ||
  grid[res1, res2 + 1] != "." ||
  grid[res1, res2 + 2] != "." ||
  grid[res1, res2 + 3] != "."){
    lastMark = grid[res1, res2];
    Console.WriteLine("\nShip overlaps with another.\nPlease select a new X.");
    res1 = Convert.ToInt32(Console.ReadLine());
 
    Console.WriteLine("\nNow select a Y.");
    res2 = Convert.ToInt32(Console.ReadLine()) - 1;
 
    Console.WriteLine("\nVertical or horizontal?\n(v/h)");
    res3 = Console.ReadLine();
    goto BattSelect;
  }
 
if(res2 + 3 > 9){
grid[res1, res2] = ".";
Console.WriteLine("\nInvalid coordinate.\nPlease select a new X.");
res1 = Convert.ToInt32(Console.ReadLine());
Console.WriteLine("\nNow select a Y.");
res2 = Convert.ToInt32(Console.ReadLine()) - 1;
Console.WriteLine("\nVertical or horizontal?\n(v/h)");
res3 = Console.ReadLine();
goto BattSelect;
}
 
grid[res1, res2] = "#";
grid[res1, res2 + 1] = "#";
grid[res1, res2 + 2] = "#";
grid[res1, res2 + 3] = "#";
batt[0] = grid[res1, res2];
batt[1] = grid[res1, res2 + 1];
batt[2] = grid[res1, res2 + 2];
batt[3] = grid[res1, res2 + 3];
break;
case "h":
 
if(grid[res1, res2] != "." ||
  grid[res1 + 1, res2] != "." ||
  grid[res1 + 2, res2] != "." ||
  grid[res1 + 3, res2] != "."){
 
    if(lastMark == "#"){
      grid[res1, res2] = "#";
    }
   
    Console.WriteLine("\nShip overlaps with another.\nPlease select a new X.");
    res1 = Convert.ToInt32(Console.ReadLine());
 
    Console.WriteLine("\nNow select a Y.");
    res2 = Convert.ToInt32(Console.ReadLine()) - 1;
 
    Console.WriteLine("\nVertical or horizontal?\n(v/h)");
    res3 = Console.ReadLine();
    goto BattSelect;
  }
 
if(res1 + 3 > 10){
grid[res1, res2] = ".";
Console.WriteLine("\nInvalid coordinate.\nPlease select a new X.");
res1 = Convert.ToInt32(Console.ReadLine());
Console.WriteLine("\nNow select a Y.");
res2 = Convert.ToInt32(Console.ReadLine());
Console.WriteLine("\nVertical or horizontal?\n(v/h)");
res3 = Console.ReadLine();
goto BattSelect;
}
grid[res1, res2] = "#";
grid[res1 + 1, res2] = "#";
grid[res1 + 2, res2] = "#";
grid[res1 + 3, res2] = "#";
batt[0] = grid[res1, res2];
batt[1] = grid[res1 + 1, res2];
batt[2] = grid[res1 + 2, res2];
batt[3] = grid[res1 + 3, res2];
break;
default:
Console.WriteLine("Invalid orient.\nTry again.");
goto Orient;
}
break;
case 2:
CruiserSelect:
switch(res3){
case "v":
 
if(grid[res1, res2] != "." ||
  grid[res1, res2 + 1] != "." ||
  grid[res1, res2 + 2] != "."){
    lastMark = grid[res1, res2];
    Console.WriteLine("\nShip overlaps with another.\nPlease select a new X.");
    res1 = Convert.ToInt32(Console.ReadLine());
 
    Console.WriteLine("\nNow select a Y.");
    res2 = Convert.ToInt32(Console.ReadLine()) - 1;
 
    Console.WriteLine("\nVertical or horizontal?\n(v/h)");
    res3 = Console.ReadLine();
    goto CruiserSelect;
  }
 
if(res2 + 2 > 9){
grid[res1, res2] = ".";
Console.WriteLine("\nInvalid coordinate.\nPlease select a new X.");
res1 = Convert.ToInt32(Console.ReadLine());
Console.WriteLine("\nNow select a Y.");
res2 = Convert.ToInt32(Console.ReadLine());
Console.WriteLine("\nVertical or horizontal?\n(v/h)");
res3 = Console.ReadLine();
goto CruiserSelect;
}
grid[res1, res2] = "#";
grid[res1, res2 + 1] = "#";
grid[res1, res2 + 2] = "#";
cruiser[0] = grid[res1, res2];
cruiser[1] = grid[res1, res2 + 1];
cruiser[2] = grid[res1, res2 + 2];
break;
case "h":
 
if(grid[res1, res2] != "." ||
  grid[res1 + 1, res2] != "." ||
  grid[res1 + 2, res2] != "."){
 
    if(lastMark == "#"){
      grid[res1, res2] = "#";
    }
   
    Console.WriteLine("\nShip overlaps with another.\nPlease select a new X.");
    res1 = Convert.ToInt32(Console.ReadLine());
 
    Console.WriteLine("\nNow select a Y.");
    res2 = Convert.ToInt32(Console.ReadLine()) - 1;
 
    Console.WriteLine("\nVertical or horizontal?\n(v/h)");
    res3 = Console.ReadLine();
    goto CruiserSelect;
  }
 
if(res1 + 2 > 10){
grid[res1, res2] = ".";
Console.WriteLine("\nInvalid coordinate.\nPlease select a new X.");
res1 = Convert.ToInt32(Console.ReadLine());
Console.WriteLine("\nNow select a Y.");
res2 = Convert.ToInt32(Console.ReadLine());
Console.WriteLine("\nVertical or horizontal?\n(v/h)");
res3 = Console.ReadLine();
goto CruiserSelect;
}
grid[res1, res2] = "#";
grid[res1 + 1, res2] = "#";
grid[res1 + 2, res2] = "#";
cruiser[0] = grid[res1, res2];
cruiser[1] = grid[res1 + 1, res2];
cruiser[2] = grid[res1 + 2, res2];
break;
default:
Console.WriteLine("Invalid orient.\nTry again.");
goto Orient;
}
break;
case 3:
 
SubSelect:
switch(res3){
case "v":
 
if(grid[res1, res2] != "." ||
  grid[res1, res2 + 1] != "." ||
  grid[res1, res2 + 2] != "."){
    lastMark = grid[res1, res2];
    Console.WriteLine("\nShip overlaps with another.\nPlease select a new X.");
    res1 = Convert.ToInt32(Console.ReadLine());
 
    Console.WriteLine("\nNow select a Y.");
    res2 = Convert.ToInt32(Console.ReadLine()) - 1;
 
    Console.WriteLine("\nVertical or horizontal?\n(v/h)");
    res3 = Console.ReadLine();
    goto SubSelect;
  }
 
if(res2 + 2 > 9){
grid[res1, res2] = ".";
Console.WriteLine("\nInvalid coordinate.\nPlease select a new X.");
res1 = Convert.ToInt32(Console.ReadLine());
 
Console.WriteLine("\nNow select a Y.");
res2 = Convert.ToInt32(Console.ReadLine());
 
Console.WriteLine("\nVertical or horizontal?\n(v/h)");
res3 = Console.ReadLine();
goto SubSelect;
}
grid[res1, res2] = "#";
grid[res1, res2 + 1] = "#";
grid[res1, res2 + 2] = "#";
sub[0] = grid[res1, res2];
sub[1] = grid[res1, res2 + 1];
sub[2] = grid[res1, res2 + 2];
break;
case "h":
if(grid[res1, res2] != "." ||
  grid[res1 + 1, res2] != "." ||
  grid[res1 + 2, res2] != "."){
 
    if(lastMark == "#"){
      grid[res1, res2] = "#";
    }
   
    Console.WriteLine("\nShip overlaps with another.\nPlease select a new X.");
    res1 = Convert.ToInt32(Console.ReadLine());
 
    Console.WriteLine("\nNow select a Y.");
    res2 = Convert.ToInt32(Console.ReadLine()) - 1;
 
    Console.WriteLine("\nVertical or horizontal?\n(v/h)");
    res3 = Console.ReadLine();
    goto SubSelect;
  }
 
if(res1 + 2 > 10){
grid[res1, res2] = ".";
Console.WriteLine("\nInvalid coordinate.\nPlease select a new X.");
res1 = Convert.ToInt32(Console.ReadLine());
Console.WriteLine("\nNow select a Y.");
res2 = Convert.ToInt32(Console.ReadLine());
Console.WriteLine("\nVertical or horizontal?\n(v/h)");
res3 = Console.ReadLine();
goto SubSelect;
}
grid[res1, res2] = "#";
grid[res1 + 1, res2] = "#";
grid[res1 + 2, res2] = "#";
sub[0] = grid[res1, res2];
sub[1] = grid[res1 + 2, res2];
sub[2] = grid[res1 + 2, res2];
break;
default:
Console.WriteLine("Invalid orient.\nTry again.");
goto Orient;
}
break;
default:
DesSelect:
switch(res3){
case "v":
 
 if(grid[res1, res2] != "." ||
  grid[res1, res2 + 1] != "."){
    lastMark = grid[res1, res2];
    Console.WriteLine("\nShip overlaps with another.\nPlease select a new X.");
    res1 = Convert.ToInt32(Console.ReadLine());
 
    Console.WriteLine("\nNow select a Y.");
    res2 = Convert.ToInt32(Console.ReadLine()) - 1;
 
    Console.WriteLine("\nVertical or horizontal?\n(v/h)");
    res3 = Console.ReadLine();
    goto DesSelect;
  }
 
if(res2 + 1 > 9){
grid[res1, res2] = ".";
 
Console.WriteLine("\nInvalid coordinate.\nPlease select a new X.");
res1 = Convert.ToInt32(Console.ReadLine());
 
Console.WriteLine("\nNow select a Y.");
res2 = Convert.ToInt32(Console.ReadLine());
 
Console.WriteLine("\nVertical or horizontal?\n(v/h)");
res3 = Console.ReadLine();
 
goto DesSelect;
}
grid[res1, res2] = "#";
grid[res1, res2 + 1] = "#";
des[0] = grid[res1, res2];
des[1] = grid[res1, res2 + 1];
break;
case "h":
 
if(grid[res1, res2] != "." ||
  grid[res1 + 1, res2] != "."){
 
    if(lastMark == "#"){
      grid[res1, res2] = "#";
    }
   
    Console.WriteLine("\nShip overlaps with another.\nPlease select a new X.");
    res1 = Convert.ToInt32(Console.ReadLine());
 
    Console.WriteLine("\nNow select a Y.");
    res2 = Convert.ToInt32(Console.ReadLine()) - 1;
 
    Console.WriteLine("\nVertical or horizontal?\n(v/h)");
    res3 = Console.ReadLine();
    goto DesSelect;
  }
 
if(res1 + 1 > 10){
grid[res1, res2] = ".";
Console.WriteLine("\nInvalid coordinate.\nPlease select a new X.");
res1 = Convert.ToInt32(Console.ReadLine());
Console.WriteLine("\nNow select a Y.");
res2 = Convert.ToInt32(Console.ReadLine());
Console.WriteLine("\nVertical or horizontal?\n(v/h)");
res3 = Console.ReadLine();
goto DesSelect;
}
grid[res1, res2] = "#";
grid[res1 + 1, res2] = "#";
break;
default:
Console.WriteLine("Invalid orient.\nTry again.");
goto Orient;
}
break;
}
shipsVal++;
if(shipsVal <= 4){
goto GridDisplay;
}
CarrSetup:
int botOrient = rnd.Next(0,2);
int carrX = 0;
int carrY = 0;
if(botOrient == 0){
carrX = rnd.Next(1,11);
carrY = rnd.Next(0,6);
if(carrY + 4 > 9){
goto CarrSetup;
}
trueGrid[carrX,carrY] = "C";
trueGrid[carrX, carrY + 1] = "C";
trueGrid[carrX, carrY + 2] = "C";
trueGrid[carrX, carrY + 3] = "C";
trueGrid[carrX, carrY + 4] = "C";
//The comment below and similar ones serve to disable revealing AI ship positions.
//Exists for debugging purposes.
/*opGrid[carrX,carrY] = "C";
opGrid[carrX, carrY + 1] = "C";
opGrid[carrX, carrY + 2] = "C";
opGrid[carrX, carrY + 3] = "C";
opGrid[carrX, carrY + 4] = "C";*/
}
if(botOrient == 1){
carrX = rnd.Next(1,7);
carrY = rnd.Next(0,10);
if(carrX + 4 > 10){
goto CarrSetup;
}
trueGrid[carrX,carrY] = "C";
trueGrid[carrX + 1, carrY] = "C";
trueGrid[carrX + 2, carrY] = "C";
trueGrid[carrX + 3, carrY] = "C";
trueGrid[carrX + 4, carrY] = "C";
//usually commented out
/*opGrid[carrX,carrY] = "C";
opGrid[carrX + 1, carrY] = "C";
opGrid[carrX + 2, carrY] = "C";
opGrid[carrX + 3, carrY] = "C";
opGrid[carrX + 4, carrY] = "C";*/
}
BattSetup:
botOrient = rnd.Next(0,2);
int battX = 0;
int battY = 0;
if(botOrient == 0){
battX = rnd.Next(1,11);
battY = rnd.Next(0,7);
if(battY + 3 > 9){
goto BattSetup;
}
if(trueGrid[battX,battY] == "C" ||
trueGrid[battX, battY + 1] == "C" ||
trueGrid[battX, battY + 2] == "C" ||
trueGrid[battX, battY + 3] == "C"){
goto BattSetup;
}
trueGrid[battX,battY] = "B";
trueGrid[battX, battY + 1] = "B";
trueGrid[battX, battY + 2] = "B";
trueGrid[battX, battY + 3] = "B";
//usually commented out
/*opGrid[battX,battY] = "B";
opGrid[battX, battY + 1] = "B";
opGrid[battX, battY + 2] = "B";
opGrid[battX, battY + 3] = "B";*/
}
if(botOrient == 1){
battX = rnd.Next(1,8);
battY = rnd.Next(0,10);
if(battX + 3 > 10){
goto BattSetup;
}
if(trueGrid[battX,battY] == "C" ||
trueGrid[battX + 1, battY] == "C" ||
trueGrid[battX + 2, battY] == "C" ||
trueGrid[battX + 3, battY] == "C"){
goto BattSetup;
}
trueGrid[battX,battY] = "B";
trueGrid[battX + 1, battY] = "B";
trueGrid[battX + 2, battY] = "B";
trueGrid[battX + 3, battY] = "B";
//usually commented out
/*opGrid[battX,battY] = "B";
opGrid[battX + 1, battY] = "B";
opGrid[battX + 2, battY] = "B";
opGrid[battX + 3, battY] = "B";*/
}
CruiserSetup:
botOrient = rnd.Next(0,2);
int cruiserX = 0;
int cruiserY = 0;
if(botOrient == 0){
cruiserX = rnd.Next(1,11);
cruiserY = rnd.Next(0,8);
if(cruiserY + 2 > 9){
goto CruiserSetup;
}
if(trueGrid[cruiserX, cruiserY] == "C" ||
trueGrid[cruiserX, cruiserY + 1] == "C" ||
trueGrid[cruiserX, cruiserY + 2] == "C" ||
trueGrid[cruiserX, cruiserY] == "B" ||
trueGrid[cruiserX, cruiserY + 1] == "B" ||
trueGrid[cruiserX, cruiserY + 2] == "B"){
goto CruiserSetup;
}
trueGrid[cruiserX,cruiserY] = "c";
trueGrid[cruiserX, cruiserY + 1] = "c";
trueGrid[cruiserX, cruiserY + 2] = "c";
//usually commented out
/*opGrid[cruiserX,cruiserY] = "c";
opGrid[cruiserX, cruiserY + 1] = "c";
opGrid[cruiserX, cruiserY + 2] = "c";*/
}
if(botOrient == 1){
cruiserX = rnd.Next(1,9);
cruiserY = rnd.Next(0,10);
if(cruiserX + 2 > 10){
goto CruiserSetup;
}
if(trueGrid[cruiserX, cruiserY] == "C" ||
trueGrid[cruiserX + 1, cruiserY] == "C" ||
trueGrid[cruiserX + 2, cruiserY] == "C" ||
trueGrid[cruiserX, cruiserY] == "B" ||
trueGrid[cruiserX + 1, cruiserY] == "B" ||
trueGrid[cruiserX + 2, cruiserY] == "B"){
goto CruiserSetup;
}
trueGrid[cruiserX, cruiserY] = "c";
trueGrid[cruiserX + 1, cruiserY] = "c";
trueGrid[cruiserX + 2, cruiserY] = "c";
//usually commented out
/*opGrid[cruiserX,cruiserY] = "c";
opGrid[cruiserX + 1, cruiserY] = "c";
opGrid[cruiserX + 2, cruiserY] = "c";*/
}
SubSetup:
botOrient = rnd.Next(0,2);
int subX = 0;
int subY = 0;
if(botOrient == 0){
subX = rnd.Next(1,11);
subY = rnd.Next(0,8);
if(subY + 2 > 9){
goto SubSetup;
}
if(trueGrid[subX, subY] == "C" ||
trueGrid[subX, subY + 1] == "C" ||
trueGrid[subX, subY + 2] == "C" ||
trueGrid[subX, subY] == "B" ||
trueGrid[subX, subY + 1] == "B" ||
trueGrid[subX, subY + 2] == "B" ||
trueGrid[subX, subY] == "c" ||
trueGrid[subX, subY + 1] == "c" ||
trueGrid[subX, subY + 2] == "c" ||
trueGrid[subX, subY] == "S" ||
trueGrid[subX, subY + 1] == "S" ||
trueGrid[subX, subY + 2] == "S"){
goto SubSetup;
}
trueGrid[subX,subY] = "S";
trueGrid[subX, subY + 1] = "S";
trueGrid[subX, subY + 2] = "S";
//usually commented out
/*opGrid[subX, subY] = "S";
opGrid[subX, subY + 1] = "S";
opGrid[subX, subY + 2] = "S";*/
}
else if(botOrient == 1){
subX = rnd.Next(1,8);
subY = rnd.Next(0,10);
if(subX + 2 > 10){
goto SubSetup;
}
if(trueGrid[subX, subY] == "C" ||
trueGrid[subX + 1, subY] == "C" ||
trueGrid[subX + 2, subY] == "C" ||
trueGrid[subX, subY] == "B" ||
trueGrid[subX + 1, subY] == "B" ||
trueGrid[subX + 2, subY] == "B" ||
trueGrid[subX, subY] == "c" ||
trueGrid[subX + 1, subY] == "c" ||
trueGrid[subX + 2, subY] == "c" ||
trueGrid[subX, subY] == "S" ||
trueGrid[subX + 1, subY] == "S" ||
trueGrid[subX + 2, subY] == "S"){
goto SubSetup;
}
trueGrid[subX, subY] = "S";
trueGrid[subX + 1, subY] = "S";
trueGrid[subX + 2, subY] = "S";
//usually commented out
/*opGrid[subX, subY] = "S";
opGrid[subX + 1, subY] = "S";
opGrid[subX + 2, subY] = "S";*/
}
DesSetup:
botOrient = rnd.Next(0,2);
int desX = 0;
int desY = 0;
if(botOrient == 0){
desX = rnd.Next(1,11);
desY = rnd.Next(0,9);
if(desY + 1 > 9){
goto DesSetup;
}
if(trueGrid[desX, desY] == "C" ||
trueGrid[desX, desY + 1] == "C" ||
trueGrid[desX, desY] == "B" ||
trueGrid[desX, desY + 1] == "B" ||
trueGrid[desX, desY] == "c" ||
trueGrid[desX, desY + 1] == "c" ||
trueGrid[desX, desY] == "S" ||
trueGrid[desX, desY + 1] == "S"){
goto DesSetup;
}
trueGrid[desX,desY] = "D";
trueGrid[desX, desY + 1] = "D";
//usually commented out
/*opGrid[desX,desY] = "D";
opGrid[desX, desY + 1] = "D";*/
}
if(botOrient == 1){
desX = rnd.Next(1,10);
desY = rnd.Next(0,10);
if(desX + 1 > 10){
goto DesSetup;
}
if(trueGrid[desX, desY] == "C" ||
trueGrid[desX + 1, desY] == "C" ||
trueGrid[desX, desY] == "B" ||
trueGrid[desX + 1, desY] == "B" ||
trueGrid[desX, desY] == "c" ||
trueGrid[desX + 1, desY] == "c" ||
trueGrid[desX, desY] == "S" ||
trueGrid[desX + 1, desY] == "S"){
goto DesSetup;
}
trueGrid[desX,desY] = "D";
trueGrid[desX + 1, desY] = "D";
//usually commented out
/*opGrid[desX,desY] = "D";
opGrid[desX + 1, desY] = "D";*/
}
goto GridDisplay;
for(int y = 0; y < 9; y++){
for(int x = 1; x < 10; x++){
if(trueGrid[x,y] != "."){
  checksVal++;
}
}
}
Attack:
//Stages player's attacks
Console.WriteLine("Select an X to attack.");
int XAttack = Convert.ToInt32(Console.ReadLine());
if(XAttack == 0){
XAttack = 10;
}
Console.WriteLine("Now select a Y.");
int YAttack = Convert.ToInt32(Console.ReadLine()) - 1;
if(YAttack == -1){
YAttack = 9;
}
if(trueGrid[XAttack, YAttack] == "S" && botSub[2] == "X"){
trueGrid[XAttack, YAttack] = ".";
}
if(trueGrid[XAttack,YAttack] != null &&
trueGrid[XAttack, YAttack] != "X" &&
trueGrid[XAttack, YAttack] != "O"){
Console.WriteLine("\nHit!\n");
botHits[botHitsVal] = "X";
botHitsVal++;
switch(trueGrid[XAttack,YAttack]){
case "C":
botCarr[botCarrHits] = "X";
if(botCarr[4] == "X"){
Console.WriteLine("You've sunk the AI's carrier!");
}
botCarrHits++;
break;
case "B":
botBatt[botBattHits] = "X";
if(botBatt[3] == "X"){
Console.WriteLine("You've sunk the AI's battleship!");
}
botBattHits++;
break;
case "c":
botCruiser[botCruiserHits] = "X";
if(botCruiser[2] == "X"){
Console.WriteLine("You've sunk the AI's cruiser!");
}
botCruiserHits++;
break;
case "S":
botSub[botSubHits] = "X";
if(botSub[2] == "X"){
Console.WriteLine("You've sunk the AI's submarine!");
}
botSubHits++;
break;
default:
botDes[botDesHits] = "X";
if(botDes[1] == "X"){
Console.WriteLine("You've sunk the AI's destroyer!");
}
botDesHits++;
break;
}
if(botHits[16] == "X"){
goto Win;
}
trueGrid[XAttack, YAttack] = "X";
opGrid[XAttack, YAttack] = "X";
}
else if(opGrid[XAttack, YAttack] == "X" ||
opGrid[XAttack, YAttack] == "O"){
Console.WriteLine("You've already shot there!");
goto Attack;
}
else if (XAttack > 10 ||
XAttack < 0 ||
YAttack > 9 ||
YAttack < -1){
Console.WriteLine("Invalid range.\nSelect a new coordinate.\n");
goto Attack;
}
else{
Console.WriteLine("\nMiss.\n");
opGrid[XAttack, YAttack] = "O";
}
BotAttack:
//Determines the AI's attack behavior
if(strikes != 0){
goto BotReturn;
}
opX = rnd.Next(1,11);
opY = rnd.Next(0,10);
if(grid[opX,opY] == "#"){
hits[hitsVal] = "X";
if(hits[16] == "X"){
goto Lose;
}
hitsVal++;
strikes++;
Console.WriteLine("\nThe AI hits!\n");
grid[opX, opY] = "X";
goto GridDisplay;
}
else if(grid[opX, opY] == "X" ||
grid[opX, opY] == "O"){
goto BotAttack;
}
else{
Console.WriteLine("\nThe AI misses.\n");
grid[opX,opY] = "O";
goto GridDisplay;
}
BotReturn:
switch(misses){
case 0:
if(opY - strikes < 0){
misses++;
strikes = 1;
goto BotReturn;
}
if(grid[opX, opY - strikes] == "#"){
hits[hitsVal] = "X";
if(hits[16] == "X"){
goto Lose;
}
hitsVal++;
Console.WriteLine("\nThe AI hits!\n");
grid[opX, opY - strikes] = "X";
strikes++;
goto GridDisplay;
}
else if(grid[opX, opY - strikes] == "X" ||
   grid[opX, opY - strikes] == "O"){
     misses++;
     strikes = 1;
     goto BotReturn;
   }
else{
Console.WriteLine("\nThe AI misses.\n");
grid[opX, opY - strikes] = "O";
misses++;
strikes = 1;
goto GridDisplay;
}
break;
case 1:
if(opY + strikes > 9){
misses++;
strikes = 1;
goto BotReturn;
}
if(grid[opX, opY + strikes] == "#"){
hits[hitsVal] = "X";
if(hits[16] == "X"){
goto Lose;
}
hitsVal++;
Console.WriteLine("\nThe AI hits!\n");
grid[opX, opY + strikes] = "X";
strikes++;
goto GridDisplay;
}
else if(grid[opX, opY + strikes] == "X" ||
   grid[opX, opY + strikes] == "O"){
     misses++;
     strikes = 1;
     goto BotReturn;
   }
else{
Console.WriteLine("\nThe AI misses.\n");
grid[opX, opY + strikes] = "O";
misses++;
strikes = 1;
goto GridDisplay;
}
break;
case 2:
if(opX - strikes < 1){
misses++;
strikes = 1;
goto BotReturn;
}
if(grid[opX - strikes, opY] == "#"){
hits[hitsVal] = "X";
if(hits[16] == "X"){
goto Lose;
}
hitsVal++;
Console.WriteLine("\nThe AI hits!\n");
grid[opX, opY + strikes] = "X";
strikes++;
goto GridDisplay;
}
else if(grid[opX - strikes, opY] == "X" ||
   grid[opX - strikes, opY] == "O"){
     misses++;
     strikes = 1;
     goto BotReturn;
   }
else{
Console.WriteLine("\nThe AI misses.\n");
grid[opX + strikes, opY] = "O";
misses++;
strikes = 1;
goto GridDisplay;
}
break;
case 3:
if(opX + strikes > 10){
strikes = 0;
goto BotAttack;
}
if(grid[opX + strikes, opY] == "#"){
hits[hitsVal] = "X";
if(hits[16] == "X"){
goto Lose;
}
hitsVal++;
Console.WriteLine("\nThe AI hits!\n");
grid[opX, opY + strikes] = "X";
strikes++;
goto GridDisplay;
}
else if(grid[opX + strikes, opY] == "X" ||
   grid[opX + strikes, opY] == "O"){
     misses++;
     strikes = 1;
     goto BotReturn;
   }
else{
Console.WriteLine("\nThe AI misses.\n");
grid[opX + strikes, opY] = "O";
misses++;
strikes = 1;
goto GridDisplay;
}
break;
default:
strikes = 0;
goto BotAttack;
}
Win:
Console.WriteLine("You sunk all the AI's ships!\nCongrats!");
goto End;
Lose:
Console.WriteLine("Game over!\nThe AI sunk all your ships!");
goto End;
End:
Console.WriteLine("\nWould you like to play again?\n(y/n)");
string ans = Console.ReadLine();
if(ans == "y" || ans == "Y" || ans == "yes" || ans == "Yes"){
goto Start;
}
else{
Console.WriteLine("\nOkay then, thanks for playing!");
}
Console.ReadKey();
}
}
}
{
  
}