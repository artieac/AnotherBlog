import RestClient from './RestClient';
import { IComment } from '../Models/IComment';

class CommentRepository {
    public async getByBlog(blogSubFolder: string, status: string = 'All'): Promise<IComment[]> {
        return RestClient.get<IComment[]>(`/api/Blog/${blogSubFolder}/Comments/${status}`);
    }

    public async updateState(blogSubFolder: string, postId: number, commentId: number, newState: string): Promise<IComment> {
        return RestClient.put<IComment>(`/api/Blog/${blogSubFolder}/BlogPost/${postId}/Comment/${commentId}/${newState}`, {});
    }

    public async delete(blogSubFolder: string, postId: number, commentId: number): Promise<void> {
        return RestClient.delete<void>(`/api/Blog/${blogSubFolder}/BlogPost/${postId}/Comment/${commentId}`);
    }
}

export default new CommentRepository();
