# AI WhatsApp Bot

An AI WhatsApp ChatBot, asnwer questions only related with eToro.

## Prerequisites

- .NET 8 SDK
- Developed in Visual Studio 2022.

## Technologies & Integrations
- APS.NET Core 8: REST APIs & gRCP(internal communication)
- Twilio
- OpenAI
- Docker: Individual containers.
- Google Cloud Run/Build: Built & Deployed in 3 web services.

## Documentation
- Project conists of 3 WebApp services and a class library for shared code.
- REST API Controller but services internally 

1. **ChatBot API:**
   - Webhook endpoint to Receive messages.
   - Integrated with Twilio to Send messages
     - Messages exceeding Twilio characters limit are broken down to more.
   - Implemented In-Memory Message-Queue  
     - Ready for integration with distributed solution.  
     - Parallelism & Retry logic implemented.
   - Calls ```QnA API``` and ```AIProvider API``` to either fetch answer from repository or request answer from AI.

2. **QuestionAnswer API:**
   - Endpoints to Create & Get Question-Answer.
   - Stores {Question, Answer, Embedding} for each Question.
   - Implemented In-Memory Repository (JSONL)
     - Ready for integration with distributed solution (Storage or Vector DB)
     - Uses ```ConcurrentDictionary<string, QnAModel>``` for fast/indexed lookups instead of file searching (like caching).
   - On service StartUp: Calls ```AIProvider API``` to populate repository with 10 most common questions and answers about eToro.
   - On GetQnA:  
     - Checks exact match with question exists.  
     - Calls ```AIProvider API``` to generate Embedding, compare agains stored embeddings (**Cosine Similarity** - with configurable Similarity threshold).
     - If no match return no answer.


3. **AIProvider API:**
   - Endpoint to receive Prompt to OpenAI and send its response.
   - Prompt Types:  
     - **QnA:** Requests answer for a single question and AI returns a string answer (gtp-3.5-turbo model).
     - **BulkQnA:** Requests answer for multiple questions and AI returns a fixes ```json structure``` with the questions and answers (gtp-4-mini).
     - **Embedding:** Requests embedding for a given string, AI returns a ```float[]``` generated (text-embedding-ada-002 model).
   - In Text Generation requests - Token Limit & Temperature is applied.
   - Implemented mechanism to receive full response if cut-off because of token limitation.
   - **Prompt**: Answers only to e-toro questions professionaly with positive attitude. Uses only information found in eToro official Help Center website.
   - Retry & Circuit Breaker Policies implemented.

## What's Next
- Implement Chat Session.
- AI Provider API handle multiple requests (parallelism). 
- Improve Embeddings similarity comparison.
- Integrate distributed storage (Vector DB for embeddings etc).
- Integrate distrtibuted messaging queue.
- Better error handling.


