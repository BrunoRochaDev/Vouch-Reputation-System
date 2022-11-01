import curses
from curses import wrapper
from network import Network

def draw_title(window):
    """Draws the ASCII title art"""

    # Gets the middle x and y to be at the center of the window
    h, w = window.getmaxyx()
    def mid_x(w : int, size :int) -> int:
        return w//2 - size//2
    mid_y = h//2

    # Adds ascii art
    window.addstr(0, mid_x(w, 36), title_ascii, curses.A_BOLD)

    window.refresh()

def draw_menu(mid_x : int, mid_y : int, key : int):
    """Draws the menu, in which a user can select the operation to do"""
    global selected_row

    window = curses.newwin(10, 40, mid_y - 10//2, mid_x - 40//2)
    window.border()
     
    # Gets the middle x and y to be at the center of the window
    h, w = window.getmaxyx()
    def mid_x(w : int, text : str) -> int:
        return w//2 - len(text)//2
    mid_y = h//2

    # Adds the window title
    window.addstr(0, mid_x(w, ' Menu '),' Menu ', curses.A_BOLD)

    # Renders the menu options
    menu = ['Add Or Remove Nodes', 'Vouch Matrix', 'Calculate Trust', 'Exit']
    for idx, row in enumerate(menu):
        x = mid_x(w, row)
        y = 3 + idx

        attr = curses.A_REVERSE if idx == selected_row else 0
        window.addstr(y, x, row, attr)

    # Get input
    if (key == curses.KEY_DOWN or chr(key) == 'j') and selected_row < len(menu): # Navigate down
        selected_row += 1
    elif (key == curses.KEY_UP or chr(key) == 'k') and selected_row > 0: # Navigate up
        selected_row -= 1

    # Clamp selected row
    selected_row = max(0, min(selected_row, len(menu)-1))

    window.refresh()

def draw_node_window(mid_x : int, mid_y : int, key : int):
    """Draws the node window, in which a user can add and remove nodes"""

    global selected_row

    # The nodes in the network
    nodes_ids = list(network.nodes.keys())

    window = curses.newwin(10, 40, mid_y - 10//2, mid_x - 40//2)
    window.border()
     
    # Gets the middle x and y to be at the center of the window
    h, w = window.getmaxyx()
    def mid_x(w : int, text : str) -> int:
        return w//2 - len(text)//2
    mid_y = h//2

    # Adds the window title
    window.addstr(0, mid_x(w, f' Nodes ({len(nodes_ids)}) '),f' Nodes ({len(nodes_ids)}) ', curses.A_BOLD)

    # If there are no nodes in the network, print it
    if not nodes_ids:
        window.addstr(mid_y, mid_x(w, 'No nodes.'),'No nodes.', curses.A_DIM)
    # If there is, print each node in a list
    else:
        y = 2
        for idx, name in enumerate(nodes_ids):
            attr = curses.A_REVERSE if idx == selected_row else 0
            window.addstr(y, mid_x(w, f'Node {name}'), f'Node {name}', attr)
            y += 1

    # Get input
    if chr(key) == ' ': # Create node
        network.create_node()
    elif (key == curses.KEY_DOWN or chr(key) == 'j') and selected_row < len(nodes_ids): # Navigate down
        selected_row += 1
    elif (key == curses.KEY_UP or chr(key) == 'k') and selected_row > 0: # Navigate up
        selected_row -= 1
    elif key == 330: # Delete node
        network.delete_node(nodes_ids[selected_row])        

    # Clamp selected row
    selected_row = max(0, min(selected_row, len(nodes_ids)-1))

    window.refresh()

def main(stdscr):
    """The main loop for drawing the TUI"""

    selected_window = 'MENU'

    curses.curs_set(0) # Make the cursor stop blinking 

    key = 0 # Starts with no input
    while True:

        h, w = stdscr.getmaxyx()
        mid_x = w//2
        mid_y = h//2

        title_window = curses.newwin(20, 38, 1, mid_x - 38//2)
        draw_title(title_window)

        # Menu
        if selected_window == 'MENU':
            draw_menu(mid_x, mid_y, key)

            end_y = mid_y + 6
            instructions = ['Up and down to nagivate through the list','Space for selecting']

        # Node window
        if selected_window == 'NODE':
            draw_node_window(mid_x, mid_y, key)

            end_y = mid_y + 10
            instructions = ['Up and down to nagivate through the list','Space for adding a node','Del for deleting highlighted node', 'Esc to go back to menu']

        # Draw the instructions for the given window
        for idx, text in enumerate(instructions):
            stdscr.addstr(end_y + idx, mid_x - len(text)//2, text)
            pass

        #stdscr.refresh()
        key = stdscr.getch() # Blocks until input is given

# Loads the ascii title art to a string
with open("ascii.txt", "r") as file:
    title_ascii = str(file.read())

# Creates the network object
network = Network()

selected_row = 0
selected_col = 0

wrapper(main)