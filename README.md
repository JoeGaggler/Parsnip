# Parsnip

A Visual Studio C# Project Extension that contains a single-file-generator for producing a parser class from a grammar file.

## Overview
A grammar file defines the structure of the target. The definition consists of a series of recursive parse rules, with each rule resolving to a C# type. The first rule in the definition is the root of the parse tree, and so the result of parse operation is the type indicated by this first rule. 

Each rule contains a union of possible productions, with each production resolving to the rule's type. A production consists of a sequence of tokens. A production that is recognized by the parser is fed to a factory method to produce an instance of the rule's type.


