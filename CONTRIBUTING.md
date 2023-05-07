# Contributing to Aris Studio

👍🎉 First off, thanks for taking the time to contribute in Aris Studio, Sensei! 🎉👍

The following is a set of guidelines for contributing to Aris Studio. These are mostly guidelines, not rules.
Use your best judgment, and feel free to propose changes to this document in a pull request.

## Table of Contents

- [Code of Conduct](#code-of-conduct "Code of Conduct")
- [I don't want to read this whole thing, I just have a question!!!](#i-dont-want-to-read-this-whole-thing-i-just-have-a-question "I don't want to read this whole thing, I just have a question!!!")
- [How to Contribute](#how-to-contribute "How to Contribute")
  - [Reporting Bugs](#reporting-bugs "Reporting Bugs")
  - [Requesting Features/Suggesting Enhancements](#requesting-featuressuggesting-enhancements "Requesting Features/Suggesting Enhancements")
  - [Developing Aris Studio](#developing-aris-studio "Developing Aris Studio")
  - [Documenting Aris Studio](#documenting-aris-studio "Documenting Aris Studio")

## Code of Conduct

This project and everyone participating in it is governed by the Aris Studio Code of Conduct. By participating, you are expected to uphold this code. Please report unacceptable behavior to t@dzaaaaaa.com.

## I don't want to read this whole thing, I just have a question!!!

If you want to just ask a question, please don't file an issue. Instead, use [Aris Studio Discussions](https://github.com/Tualin14/ArisStudio/discussions "Aris Studio Discussions") to discuss and ask a question.

## How to Contribute

This section guides you how to contribute to Aris Studio. Following these guidelines helps you understand how to contribute to Aris Studio properly.

### Reporting Bugs

Before creating bug reports, please check [Opened Issue](https://github.com/Tualin14/ArisStudio/issues?q=is%3Aopen+is%3Aissue "Opened Issue") as you might find out that you don't need to create one. When you are creating a bug report, please include as many details as possible. Fill out [the required template](/.github/ISSUE_TEMPLATE/bug_report "Bug Report template"), the information it asks for helps us resolve issues faster.

To write a good bug report, please:

- **Use a clear and descriptive title** for the issue to identify the problem.
- **Describe the exact steps which reproduce the problem** in as many details as possible.
- **Provide specific examples to demonstrate the steps**. Include links to story file or a snippets which you use in those examples.
- **Include screenshots and animated GIFs** which show you following the described steps and clearly demonstrate the problem.
- **If the problem wasn't triggered by a specific action**, describe what you were doing before the problem happened and share more information using the guidelines below.

### Requesting Features/Suggesting Enhancements

Before creating enhancement suggestions, please check [Opened Issue](https://github.com/Tualin14/ArisStudio/issues?q=is%3Aopen+is%3Aissue "Opened Issue") as you might find out that you don't need to create one. When you are creating an enhancement suggestion, please include as many details as possible. Fill in [the template](/.github/ISSUE_TEMPLATE/feature_request "Feature Request template"), including the steps that you imagine you would take if the feature you're requesting existed.

To write a good feature request, please:

- **Use a clear and descriptive title** for the issue to identify the suggestion.
- **Provide a step-by-step description of the suggested enhancement** in as many details as possible.
- **Provide specific examples to demonstrate the steps**. Include copy/pasteable snippets which you use in those examples.
- **Describe the current behavior** and **explain which behavior you expected to see** instead and why.
- **Include screenshots and animated GIFs** which help you demonstrate the steps or point out the part of Aris Studio which the suggestion is related to.
- **Explain why this enhancement would be useful** to most Sensei's and isn't something that can or should be implemented.

### Developing Aris Studio

If Sensei is a Developer and want to contributing code to Aris Studio, please follow these steps:

- We use [Editor Config](https://editorconfig.org/ "Editor Config") to keep code indentation, EOL, etc. persistent. If you're using the following code editor, please install their Editor Config extension.
  - [Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=EditorConfig.EditorConfig "Visual Studio Code Editor Config extension")
  - [Sublime Text](https://github.com/sindresorhus/editorconfig-sublime#readme "Sublime Text Editor Config extension")
- Use [Unity 2020.3.39f1](https://unity.com/releases/editor/archive#download-archive-2020 "Unity 2020.3.39f1").
- Fork and clone [Aris Studio](https://github.com/Tualin14/ArisStudio "Aris Studio") locally.
- Open Aris Studio in Unity. On the first time, there will be many errors in the console because the Spine runtime isn't present. Why don't we include [Spine](http://esotericsoftware.com/ "Spine") in Aris Studio already? Well, because of [Spine Runtimes License Agreement](http://esotericsoftware.com/spine-runtimes-license "Spine Runtimes License Agreement"), we couldn't do that :(  

  Download and import [spine-unity 3.8 for Unity 2017.1-2020.3](https://esotericsoftware.com/files/runtimes/unity/spine-unity-3.8-2021-11-10.unitypackage "spine-unity 3.8 for Unity 2017.1-2020.3"), don't forget to uncheck "Spine Examples" while importing.
- Start your journey developing Aris Studio! :)  
  To know more about Aris Studio architecture and how to contributing on those field, visit [Aris Studio Documentation - Contribute section](https://as.t14.me/en/docs/category/contribute "Aris Studio Documentation - Contribute section").

### Documenting Aris Studio

If Sensei is good at writing stuff and understanding the architecture, features, etc. of Aris Studio, Sensei can documenting them at [documentation branch](https://github.com/Tualin14/ArisStudio/tree/documentation "documentation branch"). Sensei can also write them in Sensei mother language.
