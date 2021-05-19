# Vouch Reputation System #
Vouch Reputation System, or VRS for short, is a decentralized reputation system for P2P networks written in C#.

In VRS, every account in the network can vouch for or against any other, signalizing trust or mistrust. As such, all the different accounts and their vouch relations can be displayed as a diagram of a network of nodes.

<p align="center">
  <img src="https://user-images.githubusercontent.com/44985385/118741349-70754880-b824-11eb-8db5-f3136d270a5c.png">
</p>

A vouch for relation is only deemed valid if it's reciprocal between both parties, while a vouch against is not.

A node can then be selected to be the observer node, creating a visualization of a personal network comprising itself and the nodes it connects to (even tangentially). From that, each node in it's personal network is given a reputation score between zero and one. A node's reputation score is determined by it's distance to the observer node and it's vouch relations to every other node in the network. This way, even nodes with no direct vouch relation to the observer node can be assigned an accurate reputation score.

It's important to note that reputation scores are subjective measure dependent on an observer and it's preferences and not an intrinsic property of a node.

Through VRS, a user can assess anyone's (even complete strangers) trustworthiness through their reputation score as long as someone is their personal network is connected to them.

From this set of rules explained emerges an entropic force that promotes network safety. Vouching in favor of an account which is generally considered to be untrustworthy will bring your own reputation score down to those that vouch against it. Inversely, vouching against a reputable account will also cause the same effect to those that vouch for it. This means that non-malicious users looking to maximize their own reputation in the network are incentivized to be meticulous in regards to whom they associate with and to become vigilant about troublemakers in their own personal network.

In theory, VRS ensures that a perfectly healthy social network with self interested actors with flawless judgement will eventually divide itself between a network of reputable users and non-reputable users, but such conditions are not likely to occur in reality. In practice, as long as benign users are more numerous and well connected than hostile users, it will not be feasible for malicious actors to infiltrate a cautious node's personal network and cause harm.
