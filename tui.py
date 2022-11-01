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

def draw_node_window(window):
    """Draws the node window, in which a user can add and remove nodes"""

    # The nodes in the network
    nodes_ids = list(network.nodes.keys())
    
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

    window.refresh()

def main(stdscr):
    """The main loop for drawing the TUI"""

    curses.curs_set(0) # Make the cursor stop blinking 
    while True:
        # stdscr.clear() # Clears the terminal
        # stdscr.border() # Puts a border

        h, w = stdscr.getmaxyx()
        mid_x = w//2
        mid_y = h//2

        title_window = curses.newwin(20, 38, 5, mid_x - 38//2)
        draw_title(title_window)

        node_window = curses.newwin(20, 20, mid_y - 20//2, mid_x - 20//2)
        node_window.border()
        draw_node_window(node_window)
        
        #stdscr.refresh()
        stdscr.getch() # Blocks until input is given

with open("ascii.txt", "r") as file:
    title_ascii = str(file.read())
network = Network()

network.create_node()
network.create_node()
network.create_node()

wrapper(main)