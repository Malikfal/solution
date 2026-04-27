import axios from 'axios';

const apiClient = axios.create({
  baseURL: 'https://localhost:7121/api',
  headers: { 'Content-Type': 'application/json' }
});

export default {
  // Проекты
  getProjects() { return apiClient.get('/projects'); },
  getProject(id) { return apiClient.get(`/projects/${id}`); },
  createProject(project) { return apiClient.post('/projects', project); },
  updateProject(id, project) { return apiClient.put(`/projects/${id}`, project); },
  deleteProject(id) { return apiClient.delete(`/projects/${id}`); },

  // Задачи
  getTasks(projectId = null) {
    const url = projectId ? `/tasks?projectId=${projectId}` : '/tasks';
    return apiClient.get(url);
  },
  getTask(id) { return apiClient.get(`/tasks/${id}`); },
  createTask(task) { return apiClient.post('/tasks', task); },
  updateTask(id, task) { return apiClient.put(`/tasks/${id}`, task); },
  deleteTask(id) { return apiClient.delete(`/tasks/${id}`); },

  // Проводки времени
  getTimeEntries(skip = 0, take = 100) {
    return apiClient.get(`/timeentries?skip=${skip}&take=${take}`);
  },
  getTimeEntriesByDate(date) {
    return apiClient.get(`/timeentries/date/${date}`);
  },
  getDailySummary(date) {
    return apiClient.get(`/timeentries/summary/date/${date}`);
  },
  createTimeEntry(entry) { return apiClient.post('/timeentries', entry); },
  updateTimeEntry(id, entry) { return apiClient.put(`/timeentries/${id}`, entry); },
  deleteTimeEntry(id) { return apiClient.delete(`/timeentries/${id}`); }
};