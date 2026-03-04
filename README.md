# ISO20022 Processor

A modular, pipeline-based ISO 20022 XML message processor built on .NET.  
This project provides structured message definitions, validation capabilities, and a flexible processing pipeline for handling ISO 20022 financial messages in enterprise systems.

---

## Overview

ISO 20022 is the global standard for electronic data interchange between financial institutions. It defines XML-based message schemas used across payments, securities, trade services, treasury, and reporting domains.

The **ISO20022 Processor** provides a structured foundation for:

- Parsing ISO 20022 XML messages
- Validating messages against their schema definitions
- Processing messages through a configurable pipeline
- Applying business rules and transformations
- Testing and validating financial message workflows

The solution is designed with modularity, testability, and extensibility in mind.

---

## Architecture

The repository is organized into multiple projects, each with a clear responsibility:

### `ISO20022.Definitions`

Contains strongly typed representations of ISO 20022 XML message structures.  
These definitions form the core domain model used throughout the processor.

### `ISO20022_processor_net10`

The main processing engine.  
Responsible for:

- Loading XML messages
- Mapping XML into domain objects
- Executing validation logic
- Orchestrating pipeline stages

### `Pipeline.Abstract`

Defines interfaces and abstractions for building message processing pipelines.  
This enables decoupled and extensible processing steps.

### `Pipeline.Implementation`

Concrete implementations of the pipeline components.  
Supports configurable message handling flows where each stage performs a specific task (e.g., validation, enrichment, transformation, routing).

### `Pipeline.Tests`

Unit tests ensuring correctness and reliability of pipeline behavior.

### `ISO20022.Tests`

Validation tests for ISO 20022 message definitions and processing logic.

### `Wolff.FinanceTools`

Utility components for financial domain support, such as helper methods and reusable financial logic.

---

## Processing Model

The processor follows a staged pipeline approach:

1. **Input** – Receive ISO 20022 XML message.
2. **Parsing** – Deserialize XML into strongly typed domain objects.
3. **Validation** – Validate structure and business constraints.
4. **Processing** – Execute configurable pipeline steps.
5. **Output** – Return processed message or processing result.

This design allows new processing stages to be added without modifying the core engine.

---

## Key Features

- ✔ XML-only ISO 20022 message support  
- ✔ Strongly typed message definitions  
- ✔ Schema-aligned validation  
- ✔ Modular processing pipeline  
- ✔ Clean separation of abstractions and implementations  
- ✔ Comprehensive unit testing  
- ✔ Docker support for containerized deployment  

---

## Design Principles

- **Separation of Concerns** – Definitions, processing logic, and pipeline components are isolated.
- **Extensibility** – New message types and processing stages can be added easily.
- **Testability** – Dedicated test projects validate both message handling and pipeline behavior.
- **Enterprise-Ready** – Designed for integration into financial processing systems.

---

## Use Cases

This processor can serve as a foundation for:

- Payment message validation engines
- Financial message transformation services
- ISO 20022 integration layers
- Banking middleware systems
- Regulatory reporting processors

---

## Deployment

A Dockerfile is included to allow containerized builds and deployments, making integration into CI/CD pipelines straightforward.

---

## Getting Started

1. Clone the repository.
2. Open the solution in Visual Studio or your preferred .NET IDE.
3. Build the solution.
4. Run the test projects to validate the environment.
5. Integrate the processor into your financial workflow system.

---

## Contributing

Contributions are welcome. Please ensure:

- Code follows existing architectural patterns.
- New features include unit tests.
- Changes maintain XML schema compatibility.

---

## License

Please refer to the repository for licensing information.
