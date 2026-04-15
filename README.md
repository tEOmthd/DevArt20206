# DevArt — VR Escape Game

Jeu d'évasion en réalité virtuelle développé en 48h dans le cadre d'un projet universitaire (IUT BUT Informatique, Arles).

## Concept

Le joueur progresse à travers plusieurs salles en résolvant des énigmes basées sur une mécanique centrale originale : **le clonage de mouvements**.

- Le joueur enregistre ses propres déplacements pendant 10 secondes
- Un clone rejoue ensuite ces mouvements en boucle
- Pendant ce temps, le joueur peut agir ailleurs (activer un mécanisme, pousser une caisse, poser des dynamites...)
- Les puzzles reposent sur la coordination entre le joueur et son clone

### Niveaux
- Tutoriel — prise en main de la mécanique de clonage
- Jardin — premiers puzzles en extérieur
- Prison — environnement contraint, logique plus complexe
- Finale — combinaison de toutes les mécaniques

## Stack technique

| Technologie | Usage |
|---|---|
| Unity (URP) | Moteur de jeu |
| C# | Scripts de gameplay |
| XR Interaction Toolkit 3.3 | Interactions VR |
| OpenXR 1.16 | Compatibilité casques |
| XR Hands 1.7 | Tracking des mains |
| Animation Rigging 1.4 | IK full body sur l'avatar |
| Unity Timeline | Cinématique finale |
| New Input System | Gestion des inputs VR |

## Prérequis

- Casque VR compatible OpenXR (Meta Quest, HTC Vive, Valve Index...)
- Unity 6 (URP)

## Équipe

Projet réalisé en équipe de 4 en 48h :
- Téo Mathiaud
- Thomas Salvador
- Thomas Vansteenwinckel
- Adam Georges
