import { defineStore } from 'pinia';
import api from '@/services/api';

export const useTasksStore = defineStore('tasks', {
  state: () => ({
    tasks: [],
    loading: false
  }),
  actions: {
    async fetchTasks(projectId = null) {
      this.loading = true;
      try {
        const res = await api.getTasks(projectId);
        this.tasks = res.data;
        console.log('Загружены задачи:', this.tasks);
      } catch (error) {
        console.error(error);
      } finally {
        this.loading = false;
      }
    },
    async addTask(task) {
      const payload = {
        Name: task.Name,
        ProjectId: task.ProjectId,
        IsActive: task.IsActive
      };
      const res = await api.createTask(payload);
      this.tasks.push(res.data);
    },
    async updateTask(id, task) {
      const payload = {
        Id: id,
        Name: task.Name,
        ProjectId: task.ProjectId,
        IsActive: task.IsActive
      };
      await api.updateTask(id, payload);
      await this.fetchTasks();
    },
    async deleteTask(id) {
      await api.deleteTask(id);
      this.tasks = this.tasks.filter(t => t.Id !== id);
    }
  }
});