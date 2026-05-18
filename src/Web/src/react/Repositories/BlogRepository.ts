import RestClient from './RestClient';
import { IBlog } from '@/types/blog.types';

class BlogRepository {
    public async getAll(): Promise<IBlog[]> {
        try {
            return await RestClient.get<IBlog[]>('/api/Blogs');
        } catch (error) {
            console.error('Failed to fetch blogs:', error);
            throw new Error('Could not retrieve blogs. Please try again later.');
        }
    }

    public async getById(id: number): Promise<IBlog> {
        try {
            return await RestClient.get<IBlog>(`/api/Blog/${id}`);
        } catch (error) {
            console.error(`Failed to fetch blog with id ${id}:`, error);
            throw new Error('Could not retrieve blog. Please try again later.');
        }
    }

    public async save(blog: IBlog): Promise<IBlog> {
        try {
            if (blog.Id && blog.Id > 0) {
                return await RestClient.put<IBlog>(`/api/Blog/${blog.Id}`, blog);
            } else {
                return await RestClient.post<IBlog>('/api/Blog', blog);
            }
        } catch (error) {
            console.error('Failed to save blog:', error);
            throw new Error('Could not save blog. Please check your input and try again.');
        }
    }
}

export default new BlogRepository();

