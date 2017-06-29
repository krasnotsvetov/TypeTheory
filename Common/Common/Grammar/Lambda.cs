using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Common.LambdaElements;

namespace Common.Grammar
{
    public class Lambda
    {
        private Lambda()
        { 
         
        }

        public static LambdaExpression Parse(string str)
        {
            var stream = new AntlrInputStream(str);
            var lexer = new LambdaGrammarLexer(stream);
            var parser = new LambdaGrammarParser(new CommonTokenStream(lexer));
            var visitor = new LambdaTreeVisitor();
            return visitor.Visit(parser.expression());
            
        }

        class LambdaTreeVisitor : LambdaGrammarBaseVisitor<LambdaExpression>
        {
            public override LambdaExpression Visit(IParseTree tree)
            {
                return base.Visit(tree);
            }

            public override LambdaExpression VisitExpression([NotNull] LambdaGrammarParser.ExpressionContext context)
            {
                var abstraction = context.abstraction();
                if (abstraction == null)
                {
                    return VisitApplication(context.application());
                }
                var application = context.application();
                if (application == null)
                {
                    return VisitAbstraction(abstraction);
                } else
                {
                    return new Application(VisitApplication(application), VisitAbstraction(abstraction));
                }
            }

            public override LambdaExpression VisitAbstraction([NotNull] LambdaGrammarParser.AbstractionContext context)
            {
                return new Abstraction(new Variable(context.variable().GetText()), VisitExpression(context.expression()));
            }

            public override LambdaExpression VisitApplication([NotNull] LambdaGrammarParser.ApplicationContext context)
            {
                var application = context.application();
                if (application == null)
                {
                    return VisitAtom(context.atom());
                }
                return new Application(VisitApplication(application), VisitAtom(context.atom()));
            }

            public override LambdaExpression VisitAtom([NotNull] LambdaGrammarParser.AtomContext context)
            {
                var expr = context.expression();
                if (expr != null)
                {
                    return VisitExpression(expr);
                } else
                {
                    return VisitVariable(context.variable());
                }
            }

            public override LambdaExpression VisitVariable([NotNull] LambdaGrammarParser.VariableContext context)
            {
                return new Variable(context.GetText());
            }
        }
    }
}
