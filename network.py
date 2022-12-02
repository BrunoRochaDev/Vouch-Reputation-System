from node import Node

class Network:

    MAX_DEPTH = 32

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

        # Calculate the distance from every node to the observer
        distance_to_observer = self.dijkstra_shortest_path(matrix, observer_idx)

        depth = 0 # The current depth of the propagation
        open_list = set([(observer_idx, -1)]) # The nodes to be evaulated
        influence = {}

        def weight(depth) -> float:
            return 2**(-depth)

        # Loops while depth has not been reached and there are nodes in the open list
        while depth < self.MAX_DEPTH and open_list:

            new_open_list = set() # List for the new node to be added

            for node_idx, last_idx in open_list:
                connections = matrix[node_idx]

                for other_idx, value in enumerate(connections):
                    # Cannot backtrack or stay in place
                    if other_idx == node_idx or other_idx == last_idx:
                        continue

                    # Ignore if it's not a connection
                    if value == 0:
                        continue

                    # Influence it with your vouch
                    if other_idx in influence.keys():
                        influence[other_idx].append( (depth, value) )
                    else:
                        influence[other_idx] = [ (depth, value) ]

                    # Add node connected to the open list
                    new_open_list.add( (other_idx, node_idx) )
                    
            open_list = new_open_list # Updates the open list with the new ones (minus the old ones)

            depth += 1

        # Calculate reputation score from influence
        for idx, id in enumerate(id_order):

            if idx == observer_idx:
                print(id+': 100% (Observer)')
                continue

            if idx in influence.keys(): # If the node has been influenced

                nominator = 0
                denominator = 0

                for depth, value in influence[idx]:

                    nominator += (value if value == 1 else 0) * weight(depth)
                    denominator += weight(depth)

                distance_falloff = weight(distance_to_observer[idx]-1) 

                score = float(nominator * distance_falloff) / float(denominator)
            else:
                score = 0

            print(id+':', '{:.1%}'.format(score))


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

    def dijkstra_shortest_path(self, matrix, observer_idx : int) -> list:

        # Helper function
        def min_distance(size : int, dist : list, spt : list):
            min = 1e7 # a really big number

            for vertex in range(size):
                if dist[vertex] < min and spt[vertex] == False:
                    min = dist[vertex]
                    min_index = vertex

            return min_index

        size = len(matrix[0])

        distance =  [1e7] * size # 1e7 is an arbitrary big number
        distance[observer_idx] = 0 # Distance from the observer to the observer is always 0
        spt = [False] * size # spt = shortest path tree

        for i in range(size):
            u = min_distance(size, distance, spt)

            spt[u] = True

            for vertex in range(size):
                if (matrix[u][vertex] > 0 and spt[vertex] == False and distance[vertex] > distance[u] + matrix[vertex][u]):
                    distance[vertex] = distance[u] + matrix[vertex][u]

        return distance

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
