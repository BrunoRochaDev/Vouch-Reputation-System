# Vouch Reputation System #
Vouch Reputation System, or VRS for short, is a descentralized reputation system for P2P networks written in C#.

In VRS, every account in the network can vouch for or against any other, signalizing trust or mistrust. As such, all the different accounts and their vouch relations can be displayed as a diagram of a network of nodes.

A node can then be selected to be the 'observer node'. Doing that, each node in the network is given a reputation score between zero and one subjective to the observer node. A node's reputation score is determined by it's distance to the observer node and it's vouch relations to every other node in the network. This way, even nodes with no direct relation to the observer node can be asigned a reputation score.
