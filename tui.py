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

def draw_node_window(mid_x : int, mid_y : int, key : int):
    """Draws the node window, in which a user can add and remove nodes"""

    # The nodes in the network
    nodes_ids = list(network.nodes.keys())

    window = curses.newwin(20, 20, mid_y - 20//2, mid_x - 20//2)
    window.border()
     
    # Gets the middle x and y to be at the center of the window
    h, w = window.getmaxyx()
    def mid_x(w : int, text : str) -> int:
        return w//2 - len(text)//2
    mid_y = h//2

    # Adds the window title
    window.addstr(0, mid_x(w, f'NODES─({len(nodes_ids)})'),f'NODES─({len(nodes_ids)})', curses.A_BOLD)

    # If there are no nodes in the network, print it
    if not nodes_ids:
        window.addstr(mid_y, mid_x(w, 'No nodes.'),'No nodes.', curses.A_DIM)
    # If there is, print each node in a list
    else:
        y = 2
        for idx, name in enumerate(nodes_ids):
            window.addstr(y, mid_x(w, f'Node {name}'), f'Node {name}')
            y += 1

    # Get input
    if chr(key) == ' ': # Creates node
        network.create_node()

    window.refresh()

def main(stdscr):
    """The main loop for drawing the TUI"""

    selected_window = 'NODE'

    curses.curs_set(0) # Make the cursor stop blinking 

    key = 0 # Starts with no input
    while True:

        # stdscr.clear() # Clears the terminal
        # stdscr.border() # Puts a border

        h, w = stdscr.getmaxyx()
        mid_x = w//2
        mid_y = h//2

        title_window = curses.newwin(20, 38, 1, mid_x - 38//2)
        draw_title(title_window)

        # Node window
        if selected_window == 'NODE':
            draw_node_window(mid_x, mid_y, key)

            end_y = mid_y + 20//2
            instructions = ['Up and down to nagivate through the list','Space for adding a node','Del for deleting highlighted node', 'Esc to go back to menu']

        # Draw the instructions for the given window
        for idx, text in enumerate(instructions):
            stdscr.addstr(end_y + idx, mid_x - len(text)//2, text)
            pass

        #stdscr.refresh()
        key = stdscr.getch() # Blocks until input is given

with open("ascii.txt", "r") as file:
    title_ascii = str(file.read())

network = Network()

wrapper(main)