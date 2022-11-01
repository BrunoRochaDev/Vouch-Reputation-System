from node import Node
from math import ceil

class Network:

    def __init__(self):
        self.nodes = {}
        self.node_nonce = 0 # This is used strictly to assign a non-repeating identifier to each node

    def create_node(self):
        """Creates a new node with a unique identifier."""

        # Can improve this so that supports mutliple character IDs
        def give_id(nonce : int):
            return chr(65+nonce)

        # Creates the new node
        new_id = give_id(self.node_nonce)
        self.nodes[new_id] = Node(new_id)
        self.node_nonce += 1

    def delete_node(self, id : str):
        """Deletes a node with a given identifier."""
        self.nodes.pop(id)

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

    def has_negative_connection(self, other_id) -> bool:
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