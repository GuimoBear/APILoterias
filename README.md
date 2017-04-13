# APILoterias

Rotas: GET /api/resultado/
       GET /api/resultado?concurso=1900
       POST /api/jogo/checar
       POST /api/jogo/checar?concurso=1900

Corpo da consulta do jogo: /api/jogo/checar - Matriz bidimencional de inteiros, EX: 
[
  [3,6,14,15,21,25], 
  [3,6,14,15,21,27],  
  [3,6,14,15,19,25], 
  [11,21,22,37,43,57], 
  [5, 8, 38, 40, 57, 60]
]