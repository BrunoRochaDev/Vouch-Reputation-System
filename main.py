from node import Node
from network import Network

network = Network()

adam = network.add_node("Adam")
eve = network.add_node("Eve")
cain = network.add_node("Cain")
abel = network.add_node("Abel")
peter = network.add_node("Peter")

adam.vouch("Eve", True)
adam.vouch("Cain", True)
adam.vouch("Abel", True)

eve.vouch("Adam", True)
eve.vouch("Cain", True)
eve.vouch("Abel", True)

cain.vouch("Adam", True)
cain.vouch("Eve", True)
cain.vouch("Abel", False)
cain.vouch("Peter", True)

abel.vouch("Adam", True)
abel.vouch("Eve", True)

peter.vouch("Cain", True)
peter.vouch("Abel", False)

network.calculate_reputation("Adam")
