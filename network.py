from node import Node

class Network:

    def __init__(self):
        self.nodes = {} # id -> node
        self.reputation = {} # id -> reputation score

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

        open_list = set([observer_idx]) # The nodes to be evaulated
        influence = [ [] for i in range(len(self.nodes))] # The forces influencing the score of each node
        nodes_depth = [ 1e7 for i in range (len(self.nodes)) ] # The minimum distance from the observer to each node

        # Function that transforms the depth of the node to it's weight
        def weight(depth) -> float:
            return 2**(-depth)

        def recursive_propagate(idx : int, polarity : int = 0, prev_idxs : list = [], depth : int = 0, had_against : bool = False):

            # Get the connections of this node
            connections = matrix[idx]

            # Update the depth if necessary
            if depth < nodes_depth[idx]:
                nodes_depth[idx] = depth

            # For each connection...
            for other_idx, value in enumerate(connections):

                # Ignore if it's not a connection
                if value == 0:
                    continue

                # There can only be one against connection in a chain. Stop if there's a second
                if value == -1 and had_against:
                    continue

                # Prevents backtrack / stay in place
                if other_idx == idx or other_idx in prev_idxs:
                    continue
                
                polarity_send = polarity # Polarity gets inverted after vouch againts
                against_send = had_against # If there has been a vouch against

                # If the polarity has not been set (that is, we're evaluating the observer node still)
                if polarity == 0:
                    polarity_send = value # We set the polarity as the connection
                elif value == -1:
                    polarity_send = -1
                    against_send = True

                # Influence it with your vouch
                influence[other_idx].append( (depth, polarity_send) )

                # Propagate further
                recursive_propagate(other_idx, polarity_send, prev_idxs + [idx], depth + 1, against_send)

        # Propagate trust from the observer
        recursive_propagate(observer_idx)

        # Calculate the reputation score of nodes from their influences
        for idx, influences in enumerate(influence):

            if idx == observer_idx:
                print(id_order[idx], ' : Observer')
                continue

            # If there are influences
            if influences:

                # Calculates a weighted average of the influences
                numerator = 0
                denominator = 0

                for depth, polarity in influences:
                    numerator += (1 if polarity > 0 else 0) * weight(depth)
                    #numerator += polarity * weight(depth)
                    denominator += weight(depth)
                
                score = float(numerator)/float(denominator)
                score *= weight(nodes_depth[idx] - 1) # Apllies distance falloff
            else:
                print(id_order[idx], ' : Undefined')
                continue

            print(id_order[idx], ':', '{:.1%}'.format(score))


    def create_adjacency_matrix(self, id_order : list) -> list:
        """Creates and adjancency matrix to be used in dijkstra algorithm"""

        # Creates an N*N matrix
        size = len(self.nodes)
        matrix = [[0 for i in range(size)] for j in range(size)]

        # Goes through each node...
        for node_idx, node_id in enumerate(id_order):

            # ... and each vouch
            for other_id in self.nodes[node_id].vouches.keys():
                other_idx =  id_order.index(other_id) # Gets the idx from the ordered list

                # Don't check for connections if already exists
                if matrix[node_idx][other_idx] != 0:
                    continue

                # If they vouch for each other...
                if self.has_positive_connection(node_id, other_id):
                    matrix[node_idx][other_idx] = 1
                    matrix[other_idx][node_idx] = 1
                # If they vouch against each other...
                elif self.has_negative_connection(node_id, other_id):
                    matrix[node_idx][other_idx] = -1
                    matrix[other_idx][node_idx] = -1

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
        a_vouches_for = True
        if b_id in a.vouches.keys():
            a_vouches_for = a.vouches[b_id]

        # Does B vouches for A
        b_vouches_for = True
        if a_id in b.vouches.keys():
            b_vouches_for = b.vouches[a_id]

        # Negative vouch connections are made if at least one of them vouches against the other_id
        return not (a_vouches_for and b_vouches_for)
