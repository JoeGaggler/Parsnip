# Parsnip

A dotnet tool that produces a [packrat parser](https://en.wikipedia.org/wiki/Packrat_parser) from a grammar file.

This solution also contains a Visual Studio C# Project Extension with a single-file-generator that runs the dotnet tool as a convenience.

# Overview

Parnsip provides a very convenient C# text parser generator when more robust solutions are too cumbersome. 
Parsnip generates the **complete** C# code into your target project so that your target application does not need to incorporate any a generated libraries or parser runtimes.
Since the parser is generated in your C# solution as if you had manually coded it, the parser does not incur the cost of runtime generation.

# Grammar

A grammar file defines the structure of the parser's input, and this grammar file is the input Parsnip so that it can generate the parser.

Similar to other [PEG definitions](https://en.wikipedia.org/wiki/Parsing_expression_grammar#Definition), the grammar definition consists of a series of recursive descent production rules.

The first rule in the definition is the start token.

## Rule

A rule is defined by a line that starts with a name followed by a colon, and optionally the C# type:

```
my-parse-rule: ResultType
```

Subsequent lines define possible productions (i.e. the "right hand side") for this rule. 
Recall that productions in packrat parsers are [ordered](https://en.wikipedia.org/wiki/Parsing_expression_grammar#Ambiguity_detection_and_influence_of_rule_order_on_language_that_is_matched).

The rule name starts with a letter, followed by letters, numbers, and hyphens.

If the rule does not include a C# type, then its results will be effectively "ignored" (see "match action modifiers").

## Production

A production consists of a sequence of tokens, separated by spaces:

```
token1 token2 token3
```

## Tokens

### Rule token

A **rule token** is a token that refers to another rule, which allows rules to be recursive. The token must copy the name of the rule exactly as it appears in its definition.

### Literal token

A **literal token** is a token that matches the input, literally. This token is defined by surrounding the literal with double-quotes:

```
"hello"
```

### Intrinsic token

An **intrinsic token** is a "built-in" token that recognizes common inputs using shorthand. Most intrinsic tokens are defined by surrounding its code in angle brackets, 
for example here is the production that recognizes a digit at the end of the input:

```
<#> <END>
```

Currently supported intrinsic tokens:
* `<Aa>` - a letter (upper or lower case)
* `<#>` - a digit
* `<.>` - any character (angle brackets can be omitted)
* `<-->` - optional horizontal whitespace (angle brackets can be omitted)
* `<EOS>` or `<END>` - matches the end of the input (i.e. "end of stream")
* `<EOL>` - matches the end of a line (matches CRLF or LF)
* `<EOLOS>` - matches the end of a line or the end of the stream
* `<CSTRING>` - matches a "C style string"
* `<TAB>` - matches a tab
* `<SP>` or `<SPACE>` - matches a single space

## Token modifiers

Tokens can be modified by prefixes and postfixes to change the matching behavior.

### Match action modifiers

A prefix attached to a token change the match action. If no prefix is attached, the default action is "consume", which means that the input is consumed, and then proceeds to the next token in the production rule.
These are the supported match action modifiers:
* (no prefix) - consume, which advances the input so that subsequent tokens can be matched
* `\`` - ignore, which consumes the token, but does not forward it to the parent
* `~` - fail, which immediately fails the rule if the token matches
* `&` - rewind, which matches the token, but rewinds so that the token is not yet consumed
* (no prefix) - consume

### Cardinality modifiers

A postfix attached to a token changes the cardinality of the match. If no postfix is attached, the default cardinality is "one", which means the token is matched once-and-only-once.
These are the supported cardinality modifiers:
* (no postfix) - one
* `?` - maybe, zero or one
* `*` - star, zero or more
* `+` - plus, one or more

## Special tokens

Parnsip also includes special syntax that simplifies common patterns.

### Series

It is common to want to parse a series of tokens that are separated by another token, for example a comma-separated-values of numbers.
The Parsnip syntax to match this series is to separate the item token from the separator token with a forward-slash, as shown below for a list of csv-numbers:

```
number/","
```

### Union

A union is used when you want to match equivalent sequences in a single production rule, for example if a keyword in your grammar has multiple aliases. 
In this case "equivalent" means that the number and types of the tokens in each sequence are the same.

Here is an example of rule that matches a request to add two numbers:

```
number ("plus" | "and") number
```

# Node Factory Interface

Instead of returning the entire [parse tree](https://en.wikipedia.org/wiki/Parse_tree), Parsnip generates a parser that returns a single C# type that should be simpler and more practical to the target application.
This type is defined by the first production rule in the grammar definition (i.e. the start symbol), however instantiation of this type is **not** defined in the grammar. 
Instead Parsnip generates a **Node factory interface** alongside the parser which must be implemented by the application and provided to the parser along with the input to be parsed.
This interface contains a method for each production rule, which returns the associated C# type, and has a parameter for each of the C# types that were recursively returned by methods associated with the tokens of the production rule.
Separating the instantiation from the grammar allows the grammar to remain simple, while providing the freedom to use the full-expressiveness of C#.

# Notes

Parsnip parses itself! 
Since Parsnip takes a grammar definition as input to produce a custom parser, its own grammar definition is used to produce the Parsnip parser! 
The definition and the resulting parser are located in this repo.