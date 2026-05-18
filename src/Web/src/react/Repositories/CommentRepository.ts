import RestClient from './RestClient';
import { IComment } from '@/types/comment.types';

class CommentRepository {
    public async getByBlog(blogSubFolder: string, status: string = 'All'): Promise<IComment[]> {
        try {
            return await RestClient.get<IComment[]>(`/api/Blog/${blogSubFolder}/Comments/${status}`);
        } catch (error) {
            console.error(`Failed to fetch comments for blog ${blogSubFolder}:`, error);
            throw new Error('Could not retrieve comments. Please try again later.');
        }
    }

    public async updateState(blogSubFolder: string, postId: number, commentId: number, newState: string): Promise<IComment> {
        try {
            return await RestClient.put<IComment>(`/api/Blog/${blogSubFolder}/BlogPost/${postId}/Comment/${commentId}/${newState}`, {});
        } catch (error) {
            console.error(`Failed to update comment ${commentId} state to ${newState}:`, error);
            throw new Error('Could not update comment status.');
        }
    }

    public async delete(blogSubFolder: string, postId: number, commentId: number): Promise<void> {
        try {
            return await RestClient.delete<void>(`/api/Blog/${blogSubFolder}/BlogPost/${postId}/Comment/${commentId}`);
        } catch (error) {
            console.error(`Failed to delete comment ${commentId}:`, error);
            throw new Error('Could not delete the comment.');
        }
    }
}

export default new CommentRepository();

