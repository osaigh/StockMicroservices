export {}; // Ensure this is a module
declare global {
    interface Window {
        _env_: any; // Replace 'any' with the actual type if known
    }
}