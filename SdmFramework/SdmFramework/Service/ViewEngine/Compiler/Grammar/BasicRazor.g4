grammar BasicRazor;

razorCode: modelDeclaration
    ( htmlElement | razorBlock)* ;

modelDeclaration: '@model' type ;

type: ID | genericType ;

genericType: ID '<' ID '>' ;

htmlElement: openingTag
    ( htmlElement | razorBlock | textContent | modelDataExpression | foreachStatement |  iterationVariable | globalVariable)*
    closingTag ;

textContent: ID+ ;    

modelDataExpression: '@Model.' ID ;

modelForExpression: 'Model' | 'Model.' ID;

iterationVariable: '@' ID | '@' ID '.' ID;

globalVariable:'@Global.' ID; 

openingTag: '<' ID '>' ;

closingTag: '</' ID '>' ;

blockContent: ifStatement | statement | forStatement | whileStatement | htmlElement;

razorBlock: '@{' blockContent* '}' ;

foreachStatement: '@foreach' '('(modelForExpression) ')' '{' blockContent* '}';

ifStatement: 'if' '(' expression ')' '{' (blockContent)* '}' ;

forStatement: 'for' '(' statement ';' expression ';' statement ')' '{' blockContent* '}' ;

whileStatement: 'while' '(' expression ')' '{' (blockContent)* '}';

statement: ID '=' expression ';' ;

expression
    :const                                      #constExpression
    |ID                                         #idExpression
    | '(' expression ')'                        #parenthesizedExpression
    | '!' expression                            #notExpression
    | expression multOp expression              #multiplicativeExpression    
    | expression addOp expression               #additiveExpression
    | expression compareOp expression           #comparisonExpression
    | expression boolOp expression              #booleanExpression
;

multOp: '*' | '/' ;
addOp: '+' | '-' ;

compareOp: '==' | '!=' | '>' | '<' | '>=' | '<=';

boolOp: '||' | '&&';

const: STRING | INT | FlOAT | BOOL | NULL;

INT: [0-9]+;
FlOAT :[0-9]+ '.' [0-9]+ ;
STRING: '"' ~["]* '"' ;
BOOL : 'true' | 'false';
NULL: 'null';
ID: [a-zA-Z][a-zA-Z0-9]* ;
WS: [ \t\r\n]+ -> skip ;



