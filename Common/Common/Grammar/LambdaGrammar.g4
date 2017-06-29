grammar LambdaGrammar;

/*
 * Parser Rules
 */
expression  : application abstraction | application | abstraction;
abstraction : '\\' variable '.' expression;
application : application atom | atom;
atom : '(' expression ')' | variable;
variable : ID;
/*
 * Lexer Rules
 */

ID  :  [a-zA-Z][a-zA-Z0-9\']*;
WS	:  (' ' | '\t' | '\n')+ -> skip;


