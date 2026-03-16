## Notes to the Reviewer:

* The project was developed between **March 14th and March 16th**.
* The architecture follows a good approach with a solid foundation of established practices such as **DDD, TDD, and SOLID principles**, organized in a **monorepo containing both Frontend and Backend**. This structure was chosen to allow the project to evolve in a scalable and maintainable way.
* A **background processing service** was requested. Although a solution such as **RabbitMQ with long-running workers** could be used in a more complex scenario, for this project **Hangfire** was selected to handle background jobs in a simpler and effective way for the current scope. I am not big fan of over engineering at all. 😅
* The project **does not include an authorization layer**, as it was not part of the requirements. However, it was designed in a way that allows the easy integration of modern authentication and authorization mechanisms such as **OAuth or similar approaches** if needed.
* I hope you enjoy reviewing the project. 🙂
