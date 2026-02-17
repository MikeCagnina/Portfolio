# Number Guessing Game - Swift Demo

A simple command-line number guessing game written in Swift, perfect for demonstrating Swift programming concepts.

## Game Features

- **Random Number Generation**: Uses Swift's `Int.random(in: 1...100)` to generate a random target number
- **User Input Handling**: Demonstrates `readLine()` and optional binding with `guard let`
- **Control Flow**: Shows `while` loops, `if-else` statements, and `continue`
- **String Interpolation**: Uses Swift's `\(variable)` syntax for dynamic text
- **Class Structure**: Organized in a clean `NumberGuessingGame` class
- **Error Handling**: Validates user input and provides helpful error messages

## How to Play

1. The computer thinks of a random number between 1 and 100
2. You have 10 attempts to guess the correct number
3. After each guess, you'll get a hint: "Too high!" or "Too low!"
4. If you guess correctly, you win!
5. If you run out of attempts, the game ends and shows the correct number

## Running the Game

### Option 1: Using Xcode
1. Open `NumberGuessingGame.xcodeproj` in Xcode
2. Select the "NumberGuessingGame" target
3. Press `Cmd + R` to build and run
4. The game will run in Xcode's console

### Option 2: Using Terminal
1. Open Terminal
2. Navigate to the project directory
3. Run the game:
   ```bash
   swift NumberGuessingGame/NumberGuessingGame.swift
   ```

### Prerequisites
- macOS with Xcode Command Line Tools installed
- Swift compiler (comes with Xcode)

## Swift Concepts Demonstrated

- **Classes and Objects**: `NumberGuessingGame` class with properties and methods
- **Properties**: `private var`, `private let` for encapsulation
- **Initialization**: Custom `init()` method
- **Functions**: Methods with parameters and return values
- **Control Flow**: Loops, conditionals, and early returns
- **Optionals**: Safe unwrapping with `guard let`
- **String Interpolation**: Dynamic string creation
- **Input/Output**: Console input and output operations

## Demo Tips

When presenting this code:

1. **Start with the class structure** - Show how Swift classes are defined
2. **Highlight the `init()` method** - Explain property initialization
3. **Show the `guard let` pattern** - Demonstrate Swift's safe unwrapping
4. **Point out string interpolation** - Show `\(variable)` syntax
5. **Run the game live** - Let the audience see it in action!

This game is perfect for a Swift demo because it's simple enough to understand quickly but demonstrates many fundamental Swift concepts in a practical, interactive way.
