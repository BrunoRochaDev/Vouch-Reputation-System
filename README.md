# Vouch Reputation System

## Introduction

Vouch Reputation System, or VRS for short, is a proof of concept for a decentralized reputation system for peer-to-peer networks written in Python.

It's goal is to be a community-focused solution to the uncertainty of trust between unfamiliar peers in environments without authorities. Building upon already existing social bonds between it's participants, VRS extrapolates a reputation score for each node in the network in a way that reflects it's standing on it.

In particular, VRS is suited for contexts where [complete]

## Vouching

In VRS, every entity (person, organization, etc.) is represented by a node. Nodes can vouch for or against any other, signalizing trust and mistrust respectivaly. Vouches can be retracted and altered at will. A node's vouches (both for and against) is public to all other nodes.

Vouch connections are created between nodes depending on their vouches. If two nodes reciprocally vouch for the other, a vouch for connection is formed. If at least one of them vouches against the other, a vouch against connection is formed.

As such, the entities and their vouch connections can be displayed as a diagram of a network of nodes. When we are looking at the network from the perspective of a particular node, we call it the observer node.

The reputation of nodes in the network (incluiding the ones the observer is not directly connected) can be assessed through the vouch connections there present.  

It's important to note that a node's reputation is not an intrinsic property it posesses but rather is contingent to an observer. The same node can have wildly different reputation scores when seen through the perspective of nodes with vastly different vouch connections.

In concrete terms, let's say that Alice vouches for Bob. That means that Alice trusts him and is willing to put her own reputation on the line for him. If you yourself vouches for Alice but does not know Bob, you can reasonably assume that Bob is somewhat trustworthy based on it's relationship with Alice. Reversely, Bob would seem untrustworthy to you had you vouched against Alice. Things would get more complicated however if you were connected to multiple nodes with different opinions on Bob, but the same logic applies.

Vouching is also a way for a node to make a public statement about some other's reputation while leaving it's own reputation as a collateral. That is to say, when a node vouches for some other it is essentially tying it's own reputation with the other's.

For instance, let's say you and Alice vouch for each other. If Bob were to distrust Alice and not know you, your own reputation (from his point of view) would be negatively affected by your association with Alice. Reversely, if Charlie does not know you but trusts Alice, your reputation would rise from his perspective. As before, things get more complex when the reputation of a node is affected by multiple sources.

Nodes may be connected to the observer by a varying degree of proximity. Naturally, the opinions of nodes closely connected to the observer carry more weight than those of distant ones. It's more impactful to be vouched by someone closely connected to the observer than by a distant connection.

Finally, nodes that are deemed too loosely connected (or not connected at all) to the observer have no reputation score, and as such, can be ommited as far as the observer is concerned.
