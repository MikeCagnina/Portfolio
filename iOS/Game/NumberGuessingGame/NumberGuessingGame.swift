import Foundation

class NumberGuessingGame: ObservableObject {
    @Published var targetNumber: Int
    @Published var attempts: Int
    @Published var maxAttempts: Int
    @Published var currentGuess: String = ""
    @Published var selectedGuess: Int = 1
    @Published var message: String = ""
    @Published var gameState: GameState = .welcome
    @Published var showAlert: Bool = false
    @Published var alertMessage: String = ""
    @Published var selectedGenerator: RandomGeneratorType = .mersenneTwister
    @Published var isWheelSpinning: Bool = false
    
    private var customGenerator: CustomRandomGenerator?
    
    enum GameState {
        case welcome
        case playing
        case won
        case lost
    }
    
    init() {
        self.targetNumber = 0
        self.attempts = 0
        self.maxAttempts = 10
        setupGenerator()
        generateNewTargetNumber()
    }
    
    /// Sets up the selected random number generator
    private func setupGenerator() {
        switch selectedGenerator {
        case .linearCongruential:
            customGenerator = LinearCongruentialGenerator()
        case .mersenneTwister:
            customGenerator = MersenneTwisterGenerator()
        case .xorShift:
            customGenerator = XORShiftGenerator()
        case .fibonacci:
            customGenerator = FibonacciGenerator()
        case .systemDefault:
            customGenerator = nil
        }
    }
    
    /// Generates a new target number using the selected generator
    private func generateNewTargetNumber() {
        if let generator = customGenerator {
            targetNumber = generator.nextInt(in: 1...100)
        } else {
            targetNumber = Int.random(in: 1...100)
        }
    }
    
    /// Changes the random number generator and regenerates the target number
    func changeGenerator(_ type: RandomGeneratorType) {
        selectedGenerator = type
        setupGenerator()
        if gameState == .playing {
            generateNewTargetNumber()
        }
    }
    
    /// Updates the generator when selection changes in the UI
    func updateGenerator() {
        setupGenerator()
    }
    
    /// Sets the wheel spinning state
    func setWheelSpinning(_ spinning: Bool) {
        isWheelSpinning = spinning
    }
    
    func startNewGame() {
        generateNewTargetNumber()
        attempts = 0
        currentGuess = ""
        selectedGuess = 1
        message = ""
        gameState = .playing
    }
    
    func makeGuess() {
        let guess = selectedGuess
        
        attempts += 1
        
        if guess == targetNumber {
            gameState = .won
            message = "🎉 Congratulations! You guessed the number \(targetNumber) in \(attempts) attempts!"
        } else if attempts >= maxAttempts {
            gameState = .lost
            message = "😔 Game Over! The number was \(targetNumber)."
        } else if guess < targetNumber {
            message = "📈 Too low! Try a higher number."
        } else {
            message = "📉 Too high! Try a lower number."
        }
    }
}
