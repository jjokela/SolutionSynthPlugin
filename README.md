# SolutionSythPlugin

## Rationale
In group decision-making scenarios, effectiveness is often heightened when the group's members come from diverse backgrounds and hold divergent opinions. This diversity fosters a broader range of perspectives, leading to more innovative solutions. However, in many corporate environments, decision-makers may have collaborated for extended periods, leading to a convergence of thought processes, even among individuals with varied backgrounds. Long-term collaboration can inadvertently homogenize opinions, diminishing the benefits of diversity.

Additionally, group dynamics often involve implicit biases. Dominant personalities may inadvertently steer discussions, while others might unconsciously align their differing opinions closer to a perceived consensus. These factors can subtly undermine the decision-making process, leading to less optimal outcomes.

## Purpose of the Project
This project aims to address these challenges by leveraging AI to simulate a decision-making environment that encapsulates a spectrum of perspectives and proposals. By introducing AI-generated personas with varied backgrounds, personalities, and skill sets, the project seeks to circumvent the common pitfalls of groupthink and bias in decision-making.

## Process Overview
- Creation of Personas: Initially, based on a provided scenario, the system generates a specified number of AI personas. Each persona is distinct, characterized by unique backgrounds, personalities, and skills. The unifying factor among these personas is their sufficient knowledge and capability to propose viable solutions to the given scenario.
- Proposal Generation: Each persona contributes a single proposal, accompanied by a rationale. This rationale explicates their reasoning behind the proposal, highlighting its strengths and potential weaknesses. This step ensures a well-rounded consideration of each proposed solution.
- Decision-Making Plugin: Finally, the project features a 'decision-maker' plugin. This component analyzes all the proposals, assessing them on various criteria to determine the most suitable solution. The plugin also explores the potential for synthesizing elements from multiple proposals to ascertain if a composite solution might offer superior results.

Through this process, the project endeavors to bring a novel approach to decision-making in corporate settings, ensuring a rich diversity of thought and minimizing the influence of biases and groupthink.
description & rationale

## Plugins description
- `plugins` folder contains following plugins
	```
	├── plugins
	│   ├── SolutionSynthPlugin
	│   │   ├── DecisionMaker
	│   │   ├── PersonaCreator
	│   │   └── Proposer
	│   ├── NativePlugins
	│   │   └── PersonaExtractorPlugin.cs
	```

- `PersonaCreator` - based on the given scenario, creates personas of various backgrounds and personalities to propose solutions
- `Proposer` - using the given scenario and persona, provides a proposal to a solution
- `DecisionMaker` - picks up or synthesises the best solution
- `PersonaExtractorPlugin` - a native plugin that extracts the personas from result

## Set up
- You need to add an `.env` -file to project. It should have a following content:
	```
	OPENAI_API_KEY=your-api-key-comes-here
	OPENAI_MODEL=gpt-4-1106-preview
	LOG_BASE_FOLDER_PATH=files
	```
- `OPENAI_API_KEY` is your OpenAI api key
- `OPENAI_MODEL` is the model used
- `LOG_BASE_FOLDER_PATH` is the location where the log files are stored
- `scenario.txt` -file contains a scenario to be solved. The provided scenario has a problem of company merger and acquisition. Replace the contents with your own scenario.

## Usage
- Remember to create the `.env` file and replace the `scenario.txt` -contents with your own if you wish.
- Compile the project and run from VS or run the executable (`SolutionSynthPlugin.exe`)
- At the end of the process you'll see the results in console
- Results from the prompts are stored into `files`-folder, you can override the location in `.env` -file.
