import { defineStore } from 'pinia';
import api from '@/services/api';

export const useProjectsStore = defineStore('projects', {
  state: () => ({
    projects: [],
    loading: false
  }),
  actions: {
    async fetchProjects() {
      this.loading = true;
      try {
        const res = await api.getProjects();
        this.projects = res.data;
        console.log('Загружены проекты:', this.projects);
      } catch (error) {
        console.error('Ошибка загрузки проектов:', error);
      } finally {
        this.loading = false;
      }
    },
    async addProject(project) {
      const payload = {
        Name: project.Name,
        Code: project.Code,
        IsActive: project.IsActive
      };
      const res = await api.createProject(payload);
      this.projects.push(res.data);
    },
    async updateProject(id, project) {
      const payload = {
        Id: id,
        Name: project.Name,
        Code: project.Code,
        IsActive: project.IsActive
      };
      await api.updateProject(id, payload);
      await this.fetchProjects();
    },
    async deleteProject(id) {
      await api.deleteProject(id);
      this.projects = this.projects.filter(p => p.Id !== id);
    }
  }
});