namespace RogueLib;

public static class DungeonConfig {
  public const int width  = 78;
  public const int height = 25;

  // constants for drawing
  public const char block1 = '░';
  public const char block2 = '▒';
  public const char block3 = '▓';
  public const char block4 = '█';
  public const char block5 = '█';
  public const char vert   = '│';
  public const char hor    = '─';
  public const char tlc    = '┌';
  public const char trc    = '┐';
  public const char blc    = '└';
  public const char brc    = '┘';

  //   String representation of a possible Dungeon layout.
  //   " " - solid stone, not walkable, not transparent.
  //   "." - floor, walkable and transparent
  //   "#" - tunnel, walkable and transparent
  //   "+" - door, walkable and transparent
  //   "|", "-", and any other chars - walls, not walkable, not transparent,
  //              but discoverable. 

  public const string map1 =
      """

               ┌──────┐          ┌─────────────┐
               │......│        ##+.............│            ┌───────┐
               │......│        # │.............+##          │.......│
               │......+######### └──────────+──┘ ###########+.......│
               │......│                     #               └───────┘
               └──+───┘                     #
           ########                 ##########
      ┌────+┐                     ┌─+───────┐              ┌──────────────────┐
      │.....│                     │.........│              │..................│
      │.....+#####################+.........│              │..................│
      │.....│                     │.........│              │..................│
      │.....│                     │.........│              │..................│
      │.....│                     │.........+##############+..................│
      └─+───┘                     └───+─────┘              └───────────────+──┘
        #                             #                                    #
        ######               ┌────────+──────────────┐                     #
             #             ##+.......................|                     #
             #             # |.......................|   ###################
             #             # |.......................|   #
             #             # |.......................+####
             #             # └───────────────────────┘
             ###############
      """;

  static string RIP =
      """

                    __________
                   /          \
                  /    REST    \
                 /      IN      \
                /     PEACE      \
               /                  \
               |  Dave Burchill   |
               |      110 Au      |
               |   killed by a    |
               |      snake       |
               |       2026       |
              *|     *  *  *      | *
      ________)/\\_//(\/(/\)/\//\/|_)_______
      """;
}