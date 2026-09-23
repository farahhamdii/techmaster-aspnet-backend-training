# Training Center Web API Deployment Task 🚀

This repository contains the deployment documentation and configuration for the **Training Center Web API** hosted live on MonsterASP cloud hosting.

---

## 🌐 Live Application & Documentation
* **Live API URL (Swagger UI):** [https://trainingcenterr.runasp.net/swagger/index.html](https://trainingcenterr.runasp.net/swagger/index.html)
* **Security Status:** Secured with an active SSL certificate (`HTTPS` enabled via Let's Encrypt).

---

## 🛠️ Deployment Steps & Configuration
1. **Hosting Provider:** Deployed on MonsterASP cloud servers using **WebDeploy** directly from Visual Studio.
2. **Database Setup:** 
   - Remote SQL Server instance connected via connection string updates in `appsettings.json`.
   - Database schema migrated and populated using generated migration scripts executed via the hosting control panel (`Run T-SQL`).
3. **Environment Settings:**
   - Configured production environment variables.
   - Verified end-to-end connectivity and API responses for endpoints (`Enrollments`, `Students`, `Instructors`, `Payments`, and `TrainingTracks`).
