grammar LambdaGrammar;

/*
 * Parser Rules
 */
letExpression : LET variable EQ letExpression IN letExpression | expression;
expression  : application abstraction | application | abstraction;
abstraction : '\\' variable '.' expression;
application : application atom | atom;
atom : '(' letExpression ')' | variable;
variable : ID;
/*
 * Lexer Rules
 */

LET : 'let';
IN : 'in';
ID  :  [a-zA-Z][a-zA-Z0-9\']*;
WS	:  (' ' | '\t' | '\n')+ -> skip;
EQ : '=';


