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
      } finally { this.loading = false; }
    },
    async addProject(project) {
      const res = await api.createProject(project);
      this.projects.push(res.data);
    },
    async updateProject(id, project) {
      await api.updateProject(id, project);
      await this.fetchProjects();
    },
    async deleteProject(id) {
      await api.deleteProject(id);
      this.projects = this.projects.filter(p => p.id !== id);
    }
  }
});