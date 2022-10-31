class Node:
    def __init__(self, id : str):
        self.id = id
        self.vouches = {}

    def vouch(self, other_id, polarity : bool) -> None:
        """Vouch (for or against) a particular node."""

        # Cannot vouch for itself
        if self.id == other_id:
            return

        self.vouches[other_id] = polarity

    def unvouch(self, other_id) -> None:
        """Retracts vouch given to a particular node."""
        self.vouches.pop(other_id)