<template>
  <nav class="navbar">
    <div class="nav-container">
      <div class="nav-links">
        <router-link to="/projects" class="nav-link" active-class="active">Проекты</router-link>
        <router-link to="/tasks" class="nav-link" active-class="active">Задачи</router-link>
        <router-link to="/time" class="nav-link" active-class="active">Учёт времени</router-link>
      </div>
    </div>
  </nav>
  <main class="main-content">
    <router-view />
  </main>
</template>

<style scoped>
.navbar {
  background: linear-gradient(135deg, #1e2a3a 0%, #0f1724 100%);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  position: sticky;
  top: 0;
  z-index: 100;
  backdrop-filter: blur(2px);
}

.nav-container {
  max-width: 1280px;
  margin: 0 auto;
  padding: 0 24px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  height: 70px;
}

.nav-links {
  display: flex;
  gap: 2rem;
  align-items: center;
}

.nav-link {
  color: #e2e8f0;
  text-decoration: none;
  font-size: 1.1rem;
  font-weight: 500;
  padding: 6px 0;
  position: relative;
  transition: color 0.2s;
}

.nav-link:hover {
  color: #6cb2ff;
}

/* Активная ссылка с подчёркиванием */
.nav-link.active {
  color: #5faee3;
  font-weight: 600;
}

.nav-link.active::after {
  content: '';
  position: absolute;
  bottom: -4px;
  left: 0;
  width: 100%;
  height: 2.5px;
  background: #5faee3;
  border-radius: 3px;
  box-shadow: 0 0 4px #5faee3;
  animation: slideIn 0.2s ease;
}

@keyframes slideIn {
  from {
    width: 0;
    opacity: 0;
  }
  to {
    width: 100%;
    opacity: 1;
  }
}

/* Основной контент */
.main-content {
  max-width: 1280px;
  margin: 24px auto;
  padding: 0 24px 32px;
}

/* Адаптивность для мобильных */
@media (max-width: 640px) {
  .nav-container {
    flex-direction: column;
    height: auto;
    padding: 12px 16px;
    gap: 12px;
  }
  .nav-links {
    gap: 1.2rem;
  }
  .main-content {
    padding: 0 16px 24px;
  }
}
</style>