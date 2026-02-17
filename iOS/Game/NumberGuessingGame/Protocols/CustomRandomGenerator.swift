import Foundation

/// Custom random number generator protocol
protocol CustomRandomGenerator {
    /// Generates a random integer within the specified range
    /// - Parameter range: The closed range to generate a number within
    /// - Returns: A random integer within the specified range
    func nextInt(in range: ClosedRange<Int>) -> Int
    
    /// Resets the generator to its initial state
    func reset()
}
