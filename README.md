# Vouch Reputation System

## Introduction

Vouch Reputation System, or VRS for short, is a proof of concept for a decentralized reputation system for peer-to-peer networks. This particular implementation is written in Python.

It's goal is to be a community-focused solution to the uncertainty of trust between unfamiliar peers in environments without authorities. Building upon already existing social bonds between it's participants, VRS extrapolates a numerical reputation score for each node in the network in a way that reflects it's standing on it.

In particular, VRS is suited for contexts where [complete]

## Vouching

In VRS, every entity (person, organization, etc.) is represented by a node. Nodes can vouch for or against any other, signalizing trust and mistrust respectivaly. Vouches can be retracted and altered at will. A node's vouches (both for and against) are public to all.

Vouch connections are created between nodes depending on their vouches. If two nodes reciprocally vouch for the other, a vouch for connection is formed. If at least one of them vouches against the other, a vouch against connection is formed.

As such, the entities and their vouch connections can be displayed as a diagram of a network of nodes. When we are looking at the network from the perspective of a particular node, we call it the observer node.

The reputation of nodes in the network (including those the observer does not vouch for or against) can be assessed through the vouch connections there present.  

It's important to note that a node's reputation is not an intrinsic property it posesses but rather is contingent to an observer. The same node can have wildly different reputation scores when seen through the perspective of nodes with different vouch connections.

In concrete terms, let's say that Alice vouches for Bob. That means that Alice trusts him and is willing to put her own reputation on the line for him. If you yourself vouches for Alice but does not know Bob, you can reasonably assume that Bob is somewhat trustworthy based on his relationship with Alice. Reversely, Bob would seem untrustworthy to you had you vouched against Alice. Things would be more complicated were you connected to multiple nodes with different opinions on Bob, but the same logic applies.

Vouching is also a way for a node to make a public statement about some other's reputation while leaving it's own reputation as a collateral. That is to say, when a node vouches for some other it is essentially tying it's own reputation with the other's.

For instance, let's say you and Alice vouch for each other. If Bob were to distrust Alice and not know you, your own reputation (from his point of view) would be negatively affected by your association with Alice. Reversely, if Charlie does not know you but trusts Alice, your reputation would rise from his perspective. As before, things get more complex when the reputation of a node is affected by multiple sources.

Nodes may be connected to the observer by a varying degree of proximity. Naturally, the opinions of nodes closely connected to the observer carry more weight than those of distant ones. It's more impactful to be vouched by someone closer to the observer than by a distant connection.

Not only that, but the relevance of a node's opinion is also proportional to it's own reputation score. Let's say that Alice vouches for Bob and Charlie vouches against Bob, but Alice has a higher reputation score than Charlie. As Alice has a better standing on your network, Bob's reputation should be biased towards a good reputation score, rather than to be perfectly even between good and bad.

An observer can also apply a weight to every node. Weights serve as a multiplier for the node's opinion's relevance on the observer's network. For instance, even though you vouch for both Alice and Bob, you can make it so Bob's opinions are twice as relevant as Alice's by giving him a 2x weight while leaving Alice with the default 1x weight. Weights are not public information and can be kept private.

Finally, nodes that are deemed too loosely connected (or not connected at all) to the observer have no reputation score, as there is not enough meaningful data to estimate it. As such, they can be ommited from the network as far as the observer is concerned.

## Reputation Score


