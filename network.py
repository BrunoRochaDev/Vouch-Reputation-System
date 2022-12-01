from node import Node

class Network:

    def __init__(self):
        self.nodes = {}

    def add_node(self, id : str) -> Node:
        """Creates a new node with a unique identifier."""

        # Enforces that ids are uniques
        if id in self.nodes.keys():
            raise Exception('Id is not unique. Node not created.')
        else:
            # Creates the new node
            node = Node(id)
            self.nodes[id] = node
            return node

    def delete_node(self, id : str):
        """Deletes a node with a given identifier."""
        self.nodes.pop(id)

    def calculate_reputation(self, observer_id : str):

        # Enforces that ids are uniques
        if observer_id not in self.nodes.keys():
            raise Exception('Observer does not exist. Pick a valid Id')

        # Creates a list for the nodes so that it is ordered
        id_order = list(self.nodes.keys())

        # Gets the index of the observer from the order
        observer_idx = id_order.index(observer_id)
       
        # Creates the adjanceny matrix
        matrix = self.create_adjacency_matrix(id_order)
        print(matrix[0][:])

    def create_adjacency_matrix(self, id_order : list) -> list:
        """Creates and adjancency matrix to be used in dijkstra algorithm"""

        # Creates an ordered tuple so that connections are not checked twice
        def create_tuple(a_idx, b_idx) -> tuple:
            if a_idx > b_idx:
                return (a_idx, b_idx)
            else:
                return (b_idx, a_idx)

        # Creates an N*N matrix
        size = len(self.nodes)
        matrix = [[0 for i in range(size)] for j in range(size)]

        # Goes through each node...
        for node_idx, node_id in enumerate(id_order):

            # ... and each vouch
            for other_id in self.nodes[node_id].vouches.keys():
                other_idx =  id_order.index(other_id) # Gets the idx from the ordered list

                # Creates an ordered tuple to check for repetition
                ordered_tuple = create_tuple(node_idx, other_idx)

                # Don't check for connections if already exists
                if matrix[ordered_tuple[1]][ordered_tuple[0]] != 0:
                    continue

                # If they vouch for each other...
                if self.has_positive_connection(node_id, other_id):
                    matrix[ordered_tuple[1]][ordered_tuple[0]] = 1
                # If they vouch against each other...
                elif self.has_negative_connection(node_id, other_id):
                    matrix[ordered_tuple[1]][ordered_tuple[0]] = -1

        # Print
        print(id_order)
        for i in range(size):
            for j in range(size):
                print('{:4}'.format(matrix[i][j]), end='')
            print()

        return matrix

    def has_positive_connection(self, a_id, b_id) -> bool:
        """Returns whether a positive vouch connection exists between node A and B. A positive vouch connection is made if positive vouches are reciprocal."""

        # Gets the nodes associated with the id. Returns false if at least one of them does not exist
        if a_id in self.nodes:
            a = self.nodes[a_id]
        else:
            return False
        if b_id in self.nodes:
            b = self.nodes[b_id]
        else:
            return False

        # Does A vouches for B...
        a_vouches_for = False
        if b_id in a.vouches.keys():
            a_vouches_for = a.vouches[b_id]

        # Does B vouches for A
        b_vouches_for = False
        if a_id in b.vouches.keys():
            b_vouches_for = b.vouches[a_id]

        # Positive connections are made if A and B vouch for each other
        return a_vouches_for and b_vouches_for

    def has_negative_connection(self, a_id, b_id) -> bool:
        """Returns whether a negative vouch connection exists between node A and B. A negative vouch connection is made if at least one of them vouches against the other."""

        # Gets the nodes associated with the id. Returns false if at least one of them does not exist
        if a_id in self.nodes:
            a = self.nodes[a_id]
        else:
            return False
        if b_id in self.nodes:
            b = self.nodes[b_id]
        else:
            return False

        # Does A vouches for B...
        a_vouches_for = False
        if b_id in a.vouches.keys():
            a_vouches_for = a.vouches[b_id]

        # Does B vouches for A
        b_vouches_for = False
        if a_id in b.vouches.keys():
            b_vouches_for = b.vouches[a_id]

        # Negative vouch connections are made if at least one of them vouches against the other_id
        return not (a_vouches_for and b_vouches_for)
