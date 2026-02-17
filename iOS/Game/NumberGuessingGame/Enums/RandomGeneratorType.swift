import Foundation

/// Available random number generator types
enum RandomGeneratorType: String, CaseIterable {
    case linearCongruential = "Linear Congruential"
    case mersenneTwister = "Mersenne Twister"
    case xorShift = "XORShift"
    case fibonacci = "Fibonacci"
    case systemDefault = "System Default"
    
    /// Brief description of each generator type
    var description: String {
        switch self {
        case .linearCongruential:
            return "Fast, simple LCG algorithm"
        case .mersenneTwister:
            return "High-quality MT19937 algorithm"
        case .xorShift:
            return "Fast XORShift algorithm"
        case .fibonacci:
            return "Fibonacci-based generator"
        case .systemDefault:
            return "iOS built-in random generator"
        }
    }
    
    /// Detailed explanation of how each generator works
    var detailedDescription: String {
        switch self {
        case .linearCongruential:
            return "Uses the formula: next = (a × current + c) mod m\n• Very fast and simple\n• Fast because it only requires basic integer arithmetic (multiplication, addition, modulo)\n• All operations execute in constant time O(1)\n• Uses parameters from Numerical Recipes\n• Period length: 2³²\n• Good for basic randomness needs"
        case .mersenneTwister:
            return "MT19937 algorithm with 624-word state\n• Extremely high quality randomness\n• Fast because it uses efficient bitwise operations and optimized state management\n• Most operations are simple bit shifts and XORs, which are CPU-native instructions\n• Period length: 2¹⁹⁹³⁷ - 1\n• Passes many statistical tests\n• Industry standard for simulations"
        case .xorShift:
            return "Uses bitwise XOR and shift operations\n• Very fast execution\n• Fast because bitwise operations (XOR, left/right shifts) are among the fastest CPU operations\n• No multiplication or division needed, just simple bit manipulations\n• Minimal memory access with only 4 state variables\n• Good statistical properties\n• 64-bit state with 4 variables\n• Excellent for games and real-time apps"
        case .fibonacci:
            return "Based on Fibonacci sequence mathematics\n• Uses lagged Fibonacci generator\n• Fast because it primarily uses simple addition and array indexing\n• Operations are O(1) with efficient circular buffer access\n• 55-element circular buffer\n• Good distribution properties\n• Moderate speed and quality"
        case .systemDefault:
            return "Apple's optimized random number generator\n• Uses hardware entropy when available\n• Fast because it's highly optimized at the OS level with potential hardware acceleration\n• Leverages CPU-specific instructions and optimized assembly code\n• Cryptographically secure\n• Automatically seeded\n• Best for security-critical applications"
        }
    }
}
